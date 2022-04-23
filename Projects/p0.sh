#!/bin/bash

# Make loop until user selects n / no and then exit 
yn="not n"
until [[ $yn == "n" ]]
do
# After command make sure 2 inputs are entered
if test $# -ge 2
  then
    x=$1
    y=$2
    
    echo "You entered "$x" for your first number and "$y" for your second number."
    echo
    
  elif test $# -eq 1
    
    then x=$1
    
    echo -n "Enter 2nd number: "
    read y
    echo
    
    echo "You chose "$x" for x."
    echo "You chose "$y" for y."
    echo
  
  else
    
    read -p  "Enter your name to begin: " NAME
    echo "Welcome $NAME. I'm your calculator!"
    echo -n "Enter 1st number: "
    read x
    
    echo "You chose "$x"."
    echo
    
    echo -n "Enter 2nd number: "
    read y
    
    echo "You chose "$y"."
    echo
fi
#Addition
((a=x+y))
echo "Addition: $x + $y = "$a"."
echo
#Subtraction
((b=x-y))
echo "Subtraction: $x - $y = "$b"."
echo
#Multiplication
((c=y*x))
echo "Multiplication: $x * $y = "$c"."
echo
#Division
((d=x/y))
echo "Division: $x/$y = "$d"."
echo
#Division w/remainder
((e=y%x))
echo "**note: Dividing these two numbers leaves a remainder of "$e"."
echo
echo
# number of args counted to zero skipping the first two variable checks on looping
while [ $# -gt 0 ]
   do
   shift
done

echo -n " Would you like to continue $NAME ? : (y/n) "
read yn
done