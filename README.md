# GenericsDemo

Type List , you can see the List with Angle bracket, so this list use Generics, and it has that T that T refers to the Type of elements in the list, 
![image](https://user-images.githubusercontent.com/32676744/223138482-64821c6c-c5b6-4d3e-a665-09b7652070ed.png)

so I could put int 

![image](https://user-images.githubusercontent.com/32676744/223139030-111e0023-a3b1-4b0f-8760-4d7bb352784a.png)


so above <int> is the T ( type ) here . the T allows us to specify which type this list is about. 

in earlier version of C#, ArrayList was there, which is deprecated , it was part of system.collections. do not use ArrayList 
**Generics is so much better**. Arraylist accepts objects, so we can add int, string etc to an arraylist . it requires casting to access the elements from arraylist. also this leads to run time errors if the casting is not properly done, lot of mess, lot of resource it takes for conversion from object type etc , so very inefficient, slow , not very safe. 

###Generic gives compile eror if we add a string to an int list, which is good. We like compile time error . means strongly typed.








