Preferences:
-----------------------------------------------------------------------
Look: 
	http://dotnetslackers.com/articles/net/Android-for-NET-Developers-the-Preference-Framework.aspx
	http://developer.android.com/guide/topics/ui/settings.html

Attention: Um ein XML file beim Resource-Designer zu berücksichtigen: Rechtsklick -> Eirzeugungsaktionen -> AndroidResource


Always online app
http://developer.android.com/training/basics/firstapp/starting-activity.html

Swipe-Views with tabs
http://developer.android.com/training/implementing-navigation/lateral.html#swipe-tabs
http://developer.android.com/training/design-navigation/descendant-lateral.html

Multi-pane Layouts
http://developer.android.com/design/patterns/multi-pane-layouts.html
http://developer.android.com/training/basics/fragments/fragment-ui.html

Android Preferences

Adapter Pattern


Repository Pattern
http://www.remondo.net/repository-pattern-example-csharp/


Strategy Pattern: überlegt, aber overkill --> Lösung mit Interfaces

Results from an Activity
http://developer.android.com/training/basics/intents/result.html

Unit Testing
http://docs.xamarin.com/guides/cross-platform/application_fundamentals/building_cross_platform_applications/part_6_-_testing_and_app_store_approvals
https://github.com/spouliot/Andr.Unit


Hassles/Problems
XamarinStudio stürzte je nach Version zwischen 5(beta) und 20(alpha)! mal am Tag ab. (Es musste Beta verwendet werden, da es in der Release ein Problem gab, auf verschiedenen PCs gabe - inkl meinem - das sich nich lösen ließ.)
 

Abstract class / interface - which one to choose?
--------------------------------------------------
1. Are there many classes that can be "grouped together" and described by one noun? If so, have an abstract class by the name of this noun, and inherit the classes from it. (A key decider is that these classes share functionality, and you would never instantiate just an Animal... you would always instantiate a certain kind of Animal: an implementation of your Animal base class)
Example: Cat and Dog can both inherit from abstract class Animal, and this abstract base class will implement a method void Breathe() which all animals will thus do in exactly the same fashion. (I might make this method virtual so that I can override it for certain animals, like Fish, which does not breath the same as most animals).

2. What kinds of verbs can be applied to my class, that might in general also be applied to others? Create an interface for each of these verbs.
Example: All animals can be fed, so I will create an interface called IFeedable and have Animal implement that. Only Dog and Horse are nice enough though to implement ILikeable - I will not implement this on the base class, since this does not apply to Cat.
 
Todo
