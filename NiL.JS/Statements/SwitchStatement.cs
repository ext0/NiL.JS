﻿using NiL.JS.Core.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using NiL.JS.Core;

namespace NiL.JS.Statements
{
    internal class SwitchStatement : Statement, IOptimizable
    {
        private class Case
        {
            public int index;
            public Statement statement;
        }

        internal Statement[] functions;
        private int length;
        public readonly Statement[] body;
        private Case[] cases;

        public SwitchStatement(Statement[] body, double fictive)
        {
            this.body = body;
            length = body.Length - 1;
        }

        public SwitchStatement(Statement[] body)
        {
            this.body = body;
            length = body.Length - 1;
            functions = new Function[0];
        }

        public static ParseResult Parse(string code, ref int index)
        {
            int i = index;
            if (!Parser.Validate(code, "switch", ref i))
                throw new ArgumentException("code (" + i + ")");
            while (char.IsWhiteSpace(code[i])) i++;
            if (code[i] != '(')
                throw new ArgumentException("code (" + i + ")");
            i++;
            var image = OperatorStatement.Parse(code, ref i).Statement;
            if (code[i] != ')')
                throw new ArgumentException("code (" + i + ")");
            do i++; while (char.IsWhiteSpace(code[i]));
            if (code[i] != '{')
                throw new ArgumentException("code (" + i + ")");
            do
                i++;
            while (char.IsWhiteSpace(code[i]));
            var body = new List<Statement>();
            var funcs = new List<Statement>();
            var cases = new List<Case>();
            cases.Add(null);
            while (code[i] != '}')
            {
                if (Parser.Validate(code, "case", ref i))
                {
                    do
                        i++;
                    while (char.IsWhiteSpace(code[i]));
                    var sample = OperatorStatement.Parse(code, ref i).Statement;
                    if (code[i] != ':')
                        throw new ArgumentException("code (" + i + ")");
                    i++;
                    cases.Add(new Case() { index = body.Count, statement = new Operators.StrictEqual(image, sample) });
                }
                else if (Parser.Validate(code, "default", ref i))
                {
                    if (cases[0] != null)
                        throw new InvalidOperationException("Duplicate default case in switch");
                    if (code[i] != ':')
                        throw new ArgumentException("code (" + i + ")");
                    i++;
                    cases[0] = new Case() { index = body.Count, statement = null };
                }
                if (cases.Count == 1 && cases[0] == null)
                    throw new ArgumentException("code (" + i + ")");
                var t = Parser.Parse(code, ref i, 0);
                if (t == null)
                    continue;
                if (t is Function)
                    funcs.Add(t as Function);
                else if (t is SwitchStatement)
                {
                    SwitchStatement cb = t as SwitchStatement;
                    funcs.AddRange(cb.functions);                    
                    cb.functions = new Function[0];
                    body.Add(t);
                }
                else if (t is CodeBlock)
                {
                    CodeBlock cb = t as CodeBlock;
                    funcs.AddRange(cb.functions);
                    cb.functions = new Function[0];
                    body.AddRange(cb.body);
                }
                else
                    body.Add(t);
                while (char.IsWhiteSpace(code[i]) || (code[i] == ';')) i++;
            };
            i++;
            int l = i - index;
            index = i; 
            return new ParseResult()
            {
                IsParsed = true,
                Message = "",
                Statement = new SwitchStatement(body.ToArray(), 0.0)
                {
                    functions = funcs.ToArray(),
                    cases = cases.ToArray(),
                }
            };
        }

        public override IContextStatement Implement(Context context)
        {
            return new ContextStatement(context, this);
        }

        public unsafe override JSObject Invoke(Context context)
        {
            if (functions.Length != 0)
                throw new InvalidOperationException();
            int i = cases[0].index;
            for (int j = 1; j < cases.Length; j++)
            {
                if (cases[j].statement.Invoke(context))
                {
                    i = cases[j].index;
                    break;
                }
            }
            for (; i <= length; i++)
            {
                body[i].Invoke(context);
                if (context.abort != AbortType.None)
                {
                    if (context.abort == AbortType.Break)
                        context.abort = AbortType.None;
                    return context.abortInfo;
                }
            }
            return null;
        }

        public override JSObject Invoke(Context context, JSObject _this, IContextStatement[] args)
        {
            throw new NotImplementedException();
        }

        public bool Optimize(ref Statement _this, int depth, System.Collections.Generic.HashSet<string> varibles)
        {
            var vars = new HashSet<string>();
            for (int i = 0; i < body.Length; i++)
                Parser.Optimize(ref body[i], depth + 1, vars);
            for (int i = 0; i < functions.Length; i++)
                Parser.Optimize(ref functions[i], depth + 1, vars);
            if (depth > 0)
            {
                foreach (var v in vars)
                    varibles.Add(v);
                if (body.Length == 1)
                    _this = body[0];
            }
            else
                throw new InvalidOperationException();
            return false;
        }
    }
}