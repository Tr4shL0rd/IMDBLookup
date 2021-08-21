#!/bin/bash
function build() {
	mkdir ~/imdbLookup
	dotnet build -o ~/imdbLookup/ -c Release -v q 
	mv ~/imdbLookup/csharp ~/imdbLookup/imdbLookup 
	echo "Binary File Directory -> $HOME/imdbLookup/"
}
build