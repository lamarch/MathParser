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

Of course, you can create your own *context* system.

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
		* *soon* : `Python` 

## Incomming features :

* Basic operator : `^`

* Properties :
	* `Python`

* Functions :
	* `Python`
