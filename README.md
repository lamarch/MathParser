# MathsParser
A little maths expression engine, for the fun.

It's very simple to use.

```csharp
Compiler compiler = new Compiler();

string code = "1 + 2(8/1.5)"

var result = compiler.Evaluate(code);

if(result.HasErrors){
	//Some errors at Compile-time (Parser and Lexer) or Run-time
	List<Error> errors = result.Errors;
}else{
	//The result of the expression
	double result = result.Value;
}
```

- This parser support a contextual execution. You can add and remove functions and properties, from other parsed maths-expression, from `C# assemblies`, or from `Python` code. Of course, you can create your own *context* system to support `JavaScript` code or whatever you want. 

- The execution engine uses a stack memory, to allow local variable (like parameters). When a *callable* (function or property) is defined, it's allocated on the global segment, available everywhere.

## Features :

* Basic operators : `+ - * / %`

* Unary operators : `+ -`

* Parenthesis

* Implicit `*` operator before `(`

* Context evaluation :

	* Properties :
		* `C# native`
		* `Expression-based`
		* *soon* : `Python`

	* Functions : 
		* `C# native`
		* `Expression-based`
		* `Python` 

## Incoming features :

* Basic operator : `^`

* Properties :
	* `Python`

