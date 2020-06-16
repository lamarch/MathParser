using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MathParser.Parsing.Nodes;

namespace MathParser.Execution.Injection
{
    public class SegmentInjector
    {
        private readonly Segment segment;

        public SegmentInjector (Segment segment)
        {
            this.segment = segment;
        }

        public Result<List<Function>> AddTypeRefMethods (Type t, object rtObjRef)
        {
            Result<List<Function>> result = new Result<List<Function>>(new List<Function>());

            var fcts = from m in t.GetMethods(BindingFlags.Public | (rtObjRef == null ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.FlattenHierarchy)
                       where m.ReturnType == typeof(double)
                       where m.GetParameters().All(p => p.ParameterType == typeof(double))
                       select m;

            foreach ( var fct in fcts ) {
                result.MergeIf(this.segment.AddFunction(ReflectionHelper.GenerateFunction(fct, rtObjRef)), (r, list) => list.Add(r));
            }

            return result;
        }

        public Result<List<Property>> AddTypeRefProperties (Type t, object rtObjRef)
        {
            var result = new Result<List<Property>>(new List<Property>());
            var props = from f in t.GetFields(BindingFlags.Public | (rtObjRef == null ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.FlattenHierarchy)
                        where f.FieldType == typeof(double)
                        select f;

            foreach ( var prop in props ) {

                result.MergeIf(this.segment.AddProperty(new Property(prop.Name, (double)prop.GetValue(rtObjRef))), (r, list) => list.Add(r));
            }

            return result;
        }

        public Result<Function> LoadMethod (MethodInfo info, object rtObjRef)
        {
            try {
                return this.segment.AddFunction(ReflectionHelper.GenerateFunction(info, rtObjRef));
            }
            catch ( Exception e ) {
                return new Result<Function>(new Error(e, -1, $"Impossible d'ajouter la fonction par reference de type '{info.Name}'.", Error.FormatSource("Segment", true)));
            }
        }

        public Result<List<Function>> LoadMethods (IEnumerable<MethodInfo> methods, object rtObjRef)
        {
            var result = new Result<List<Function>>(new List<Function>());

            foreach ( var method in methods ) {
                var r = LoadMethod(method, rtObjRef);
                result.MergeIf(r, (val, list) => list.Add(val));
            }

            return result;
        }

        public Result<List<Function>> AddCSAssemblyReference (Assembly asm)
        {
            var methods = from t in asm.GetTypes()
                          from m in t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                          where m.GetCustomAttribute<MathImplAttribute>() != null
                          where m.ReturnType == typeof(double)
                          where m.GetParameters().All(p => p.ParameterType == typeof(double))
                          select m;

            return LoadMethods(methods, null);
        }


        public Result<Function> AddExpression (string name, Expression expression, params string[] parameters) => this.segment.AddFunction(new Function(name, ctx => expression.Eval(ctx), parameters));


    }
}
