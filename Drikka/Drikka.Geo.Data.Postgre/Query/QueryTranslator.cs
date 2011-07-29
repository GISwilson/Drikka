﻿using System;
using System.Collections.Generic;
using System.Text;
using Drikka.Geo.Data.Contracts.Mapping;
using Drikka.Geo.Data.Contracts.Query;
using Drikka.Geo.Data.Query.Connectors;
using Drikka.Geo.Data.Query.Operators;

namespace Drikka.Geo.Data.Postgre.Query
{
    public class QueryTranslator : IQueryTranslator
    {
        private readonly IMappingManager _mappingManager;

        private readonly IDictionary<Type, Func<IOperator, string, object, string>> _operators;

        private readonly IDictionary<Type, Func<IConnector, string>> _connectors;

        public QueryTranslator(IMappingManager mappingManager)
        {
            this._mappingManager = mappingManager;
            this._operators = MapOperators();
            this._connectors = MapConnectors();
        }

        public string Translate<T>(IQuery<T> query)
        {
            var map = this._mappingManager.GetMapping(query.QueriedType);
            var sql = new StringBuilder();
            var connectors = query.Connectors.GetEnumerator();

            foreach (var criteria in query.Criterias)
            {
                var attr = map.GetByAttributeName(criteria.Predicate.Field.Name);
                var clause = this._operators[criteria.Operator.GetType()].Invoke(criteria.Operator, attr.FieldName,
                                                                                 criteria.Value);
                sql.Append(clause);
                sql.Append(" ");

                if (connectors.MoveNext())
                {
                    var conn = connectors.Current;
                    sql.Append(this._connectors[conn.GetType()].Invoke(conn));
                    sql.Append(" ");
                }
            }

            return sql.ToString();
        }



        private static IDictionary<Type, Func<IOperator, string, object, string>> MapOperators()
        {
            var map = new Dictionary<Type, Func<IOperator, string, object, string>>();

            map.Add(typeof(Equal), TranslateToSqlMethods.TranslateOperatorEqual);
            map.Add(typeof(NotEqual), TranslateToSqlMethods.TranslateOperatorNotEqual);
            map.Add(typeof(GreaterThan), TranslateToSqlMethods.TranslateOperatorGreaterThan);

            return map;
        }

        private static IDictionary<Type, Func<IConnector, string>> MapConnectors()
        {
            var map = new Dictionary<Type, Func<IConnector, string>>();

            map.Add(typeof(And), TranslateToSqlMethods.TranslateConnectorAnd);
            map.Add(typeof(Or), TranslateToSqlMethods.TranslateConnectorOr);

            return map;
        }
    }
}