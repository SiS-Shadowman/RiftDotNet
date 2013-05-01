#pragma once

using namespace System;




public ref class Sword
{
public:

	Sword(){
		Console::WriteLine("Sword()");
	}

	~Sword(){
		Console::WriteLine("~Sword()");
	}

	!Sword(){
		Console::WriteLine("!Sword()");
	}
};

public ref class Link
{
public:

	Link(){
		Console::WriteLine("Link()");
	}

	~Link(){
		Console::WriteLine("~Link()");
	}

	!Link(){
		Console::WriteLine("!Link()");
	}

private:

	Sword _sword;
};
