# Array2DEditor

Use this if you want to deal with 2D arrays easily within the inspector of Unity.

## Getting Started

For a quick import into an existing project, just get the [UnityPackage](Array2DEditorPackage.unitypackage).

The Array2DEditor folder is an empty project with only the plugin imported and some examples! :)

Then, when you're in your project:

- Right click in your Project or Hierarchy window, or go to the Assets menu tab.
- Go to Create -> Array2D and select the type of your choice (_bool_, _int_, _float_, _string_).
- A new file is created, you can freely change its values!
- Reference that file in one of your scripts, and call its method GetCells() to get the content of the array.
- When you do this, don't forget to add _using Array2DEditor_ on top of your script.
- You can check the ExampleScript if you have trouble understanding how it works and how it can be useful.

## Screenshots

![Example 1](Screenshots/Example_1.PNG)
![Example 2](Screenshots/Example_2.PNG)
![Example 3](Screenshots/Example_3.PNG)

## Notes

* Last tested with [Unity 2018.3.8f1](https://unity3d.com/unity/whats-new/2018.3.8).

## Authors

* **[Arthur Cousseau](https://www.linkedin.com/in/arthurcousseau/)**

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details