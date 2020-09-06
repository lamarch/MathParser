using System;
using System.Runtime.CompilerServices;

namespace MathParser
{
    public class ErrorCodes
    {
        /*
         * 
         * 
         * 
         * Error code pattern :
         * 
         *          0 = application
         *          x
         *          1 = known error (0 as unknown error)
         *          0
         *          a = module (1 = execution context, 2 = loader...)
         *          b = error index
         * 
         * 
         */



        /*
         * ----------
         * 
         * Execution Context
         * 
         * ----------
         */

        public static Error STACK_OVERFLOW ([CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Dépassement de pile",
            message: "La limite de la pile des appels a été dépassée.",
            source: "ExecutionContext.Call",
            code: "0x1011",
            position: -1,
            isRuntime: true,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: new StackOverflowException()
            );









        /*
         * ----------
         * 
         * Loader
         * 
         * ----------
         */
        public static Error FUNCTION_CALL (string funcName, string loaderName, Exception e, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Appel de fonction impossible",
            message: $"Impossible d'invoquer la fonction dynamique '{funcName}'.",
            source: $"Loader.CallFunction({loaderName})",
            code: "0x1021",
            position: -1,
            isRuntime: true,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: new MethodAccessException()
            );

        public static Error FUNCTION_CREATION (string funcName, Exception e, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Création de fonction impossible",
            message: $"Impossible de créer la fonction '{funcName}'.",
            source: "Loader.GetFunctions",
            code: "0x1022",
            position: -1,
            isRuntime: false,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: e
            );








        /*
         * ----------
         * 
         * Segment Injector
         * 
         * ----------
         */
        public static Error LOAD_FUNCTIONS (string loaderName, Exception e, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Chargement des fonctions impossible",
            message: $"Impossible de charger les fonctions du loader '{loaderName}'.",
            source: "SegmentInjector.Load",
            code: "0x1031",
            position: -1,
            isRuntime: false,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: e
            );
        public static Error LOAD_PROPERTIES (string loaderName, Exception e, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Chargement des propriétés impossible",
            message: $"Impossible de charger les propriétés du loader {loaderName}.",
            source: "SegmentInjector.Load",
            code: "0x1032",
            position: -1,
            isRuntime: false,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: e
            );









        /*
         * ----------
         * 
         * Segment
         * 
         * ----------
         */
        public static Error FUNCTION_NOT_FOUND (string funcName, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Fonction introuvable",
            message: $"La fonction '{funcName}' n'existe pas ou n'est pas atteignable, impossible de l'appeler.",
            source: "Segment.GetFunction",
            code: "0x1041",
            position: -1,
            isRuntime: true,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: null
            );


        public static Error ADJUST_FUNCTION_NOT_FOUND (string funcName, int funcArgCount, string newFuncName, int newArgCount, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Fonction introuvable, possible remplacent",
            message: $"La fonction '{funcName}' n'existe pas avec {funcArgCount} paramètres, cependant, la fonction '{newFuncName}' existe avec {newArgCount} paramètres.",
            source: "Segment.GetFunction",
            code: "0x1042",
            position: -1,
            isRuntime: true,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: null
            );
        public static Error PROPERTY_NOT_FOUND (string propName, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Propriété introuvable",
            message: $"La propriété '{propName}' n'existe pas ou n'est pas atteignable, impossible de l'appeler.",
            source: "Segment.GetProperty",
            code: "0x1043",
            position: -1,
            isRuntime: true,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: null
            );

        public static Error EXISTING_FUNCTION (string funcName, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Fonction déjà existante",
            message: $"Impossible de créer la fonction '{funcName}' car elle existe déjà.",
            source: "Segment.AddFunction",
            code: "0x1044",
            position: -1,
            isRuntime: false,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: null
            );
        public static Error EXISTING_PROPERTY (string propName, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Propriété déjà existante",
            message: $"Impossible de créer la propriété '{propName}' car elle existe déjà.",
            source: "Segment.AddProperty",
            code: "0x1045",
            position: -1,
            isRuntime: false,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: null
            );









        //Parser
        public static Error VALUE_EXPECTED (int position, Token found, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Valeur attendue",
            message: $"Une valeur est attendue à la place de '{found}'.",
            source: "Parser.ParseLeaf",
            code: "0x1051",
            position: position,
            isRuntime: false,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: null
            );

        public static Error TOKEN_EXPECTED (string source, Token expType, Token found, int position, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Symbole inattendu",
            message: $"Le symbole de type '{expType}' est attendu, mais le symbole de type '{found}' a été trouvé à sa place.",
            source: source,
            code: "0x1052",
            position: position,
            isRuntime: false,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: null
            );










        //Lexer
        public static Error UNKNOWN_CHAR (int position, [CallerLineNumber] int l = -1, [CallerMemberName] string m = "unknown", [CallerFilePath] string fp = "unknown") => new Error(
            name: "Signe interdit",
            message: "Le signe trouvé est inconnu ou interdit.",
            source: "Lexer.Lex",
            code: "0x1061",
            position: position,
            isRuntime: false,
            calleLine: l,
            memberName: m,
            filePath: fp,
            exception: null
            );

    }
}
