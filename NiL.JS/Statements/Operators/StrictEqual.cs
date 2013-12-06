﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NiL.JS.Core;
using NiL.JS.Core.BaseTypes;

namespace NiL.JS.Statements.Operators
{
    internal unsafe class StrictEqual : Operator
    {
        public StrictEqual(Statement first, Statement second)
            : base(first, second)
        {

        }

        public override JSObject Invoke(Context context)
        {
            var temp = first.Invoke(context);

            var lvt = temp.ValueType;
            if (lvt == ObjectValueType.Int)
            {
                var l = temp.iValue;
                temp = second.Invoke(context);
                if (lvt != temp.ValueType)
                    return false;
                return l == temp.iValue;
            }
            if (lvt == ObjectValueType.Double)
            {
                var l = temp.dValue;
                temp = second.Invoke(context);
                if (lvt != temp.ValueType)
                    return false;
                return l == temp.dValue;
            }
            if (lvt == ObjectValueType.Bool)
            {
                var l = temp.bValue;
                temp = second.Invoke(context);
                if (lvt != temp.ValueType)
                    return false;
                return l == temp.bValue;
            }
            if (lvt == ObjectValueType.Statement)
            {
                var l = temp.oValue;
                temp = second.Invoke(context);
                if (lvt != temp.ValueType)
                    return false;
                return l == temp.oValue;
            }
            if (lvt == ObjectValueType.Object)
            {
                var l = temp.oValue;
                temp = second.Invoke(context);
                if (lvt != temp.ValueType)
                    return false;
                return l == temp.oValue;
            }
            if (lvt == ObjectValueType.String)
            {
                var l = temp.oValue;
                temp = second.Invoke(context);
                if (lvt != temp.ValueType)
                    return false;
                return l.Equals(temp.oValue);
            }
            if (lvt == ObjectValueType.Undefined)
            {
                var l = temp.dValue;
                temp = second.Invoke(context);
                if (lvt != temp.ValueType)
                    return false;
                return true;
            }
            throw new NotImplementedException();
        }
    }
}