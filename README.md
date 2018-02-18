# WFS-ConsoleSystem
*A developer console library for .NET 3.5+*

## Features
Designed for (but not dependant on) Unity, this library offers a Source/Id style developer console backend. It features a number of enhancements over those older implementations, including:
- Full use of C# generics. Just about anything fully implementing IConvertible is able to be used in a console object.
- Special support for enums, using the special ConsoleEnumVariable<> class.
- Configurable config file syntax. The default is inspired by Valve's style of .cfg file, but makes use of = signs like in .inis for readability.
- Partial support for declaring commands/variables at runtime.

Old, familar features are present too:
- Console variables and commands
- Loading, execution and saving of config files.

## Limitations
The library is fairly unfinished, and may be somewhat buggy. It is also lacks some features:
- Performance: the console system has not be optimised or even tested for performance. It makes use of C#'s Convert.ChangeType(), which is probably not the fastest method on earth. Variables *do* however cache their values, and the system has been used to access enum variables every call of Unity's Update() on a script without any issue.
- Does not implement all of the usual Source/Id commands. *bind* is the main one missing, and it might take a bit more work on the backend to implement it.
- This is not a console frontend! No GUI is provided, apart from the Test App included in the repo. You are responsible for implementing the library's functionality yourself, although one day we might release our own Unity integration solution.

## Implementation
The console system offers both console variables and console commands for use. They can be implemented in their basic form as follows:

```CSharp
static ConsoleCommand aCommand = new ConsoleCommand("a_command", CommandMethod, "A command with no parameters"); // This is a console command.
static ConsoleVariable<int> aVariable = new ConsoleVariable<int>("a_variable", 10, "An integer console variable"); // This is a console variable.
```

However, just using these features only are quite limiting, so a number of other types are provided.

### Console Commands
There are three main kinds of console commands available: no-parameter, 1/2/3 parameter or generic parameter.

```CSharp
static ConsoleCommand noParameters; // Method that takes no parameters
static ConsoleCommand<int> oneParameter; // Method that takes 1 integer as a parameter
static ConsoleCommand<int, int> twoParameters; // Method that takes 2 integers as parameters
static ConsoleCommand<int, int, int> threeParameters; // Method that takes 3 integers as parameters
static ConsoleCommandGeneric genericParameters; // Method that takes a string[] as a parameter
```

More parameter command types could be easily implemented, but typing them out takes a while and they have never been needed.

### Multicast Commands
These function almost exactly like normal console commands, but allow more than 1 action to be bound to a command. Actions can also be unbound at will.

```CSharp
static ConsoleMulticastCommand noParams;
static ConsoleMulticastCommand<int> oneParam;
static ConsoleMulticastCommand<int, int> twoParams;
static ConsoleMulticastCommand<int, int, int> threeParams;
static ConsoleMulticastCommandGeneric genericParams;
```

Actions can be bound like so:
```CSharp
multiCommand.Add(MethodOfInstanceObject);
```

...and removed like so:
```CSharp
multiCommand.Remove(MethodOfInstanceObject);
```

This can be used to allow multiple game entities to respond to commands that are static members of their class.

### Console Variables
There are three types of console variable:

```CSharp
static ConsoleVariable<int>; // Of integer type.
static ConsoleEnumVariable<MyEnum>; // Of custom enum
static ConsoleCallbackVariable<int>; // Of type integer
```

ConsoleVariable<> is quite self-explanatory, but ConsoleEnumVariable and ConsoleCallbackVariable are more advanced in their features.

ConsoleEnumVariable<> is a special type of console variable that must be used if you wish to use any enums in a variable. They allow the console parser to set their values by name rather than the numeric value behind the enum.

ConsoleCallbackVariable<> combines the features of both commands and variables, allowing a function to be called when the value of the variable is changed. When constructing the command, both the function and default value must be specified.

## Custom Console Objects
It should be possible to implement custom console objects by extending the ConsoleBase class. Adding custom functionality can be achieved by overriding the Invoke(string[]) method, which is called when the object is entered into the console. The array will contain any arguments specified along with the object's name. For example, in the command string "execute_custom_object: param1, param2", the strings "param1" and "param2" would be passed in the array.