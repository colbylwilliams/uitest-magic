# uitest-magic
A utility to quickly add IDs to UI elements of existing apps that can be used with UITest and Xamarin Test Cloud.

How to use
---

Pull down the solution and build `UITestIdGenerator.Mac` in Xamarin Studio.  When the app launches, simply drag & drop your `.storyboard` or `.xib` file is into the app's window.  

This will do two things:

1. Populates the accessibilityIdentifier or accessibilityLabel property on all the "interactive" UI elements in file with a descriptive unique identifier.

2. Generates a file named `UIElements.cs` that lists all of the identifiers to use when writing your UITests.  UIElements.cs is saved in the same directory as the .storyboard or .xib file.

To use `UIElements.cs`, you must first include it in your __UITest project__.  You can immediately use it to query any UI element in the `.storyboard` or `.xib`.

For example, if the storyboard has a subclass of UIViewController named LoginViewController with a child UIButton titled "Login".  You'd query the login button with: 

`app.Query(x => x.Marked(UIElements.LoginViewController.button_login));`

_(with Intellisense of course)_

TODO
----

- Android
- AddIn for Xamarin Studio
