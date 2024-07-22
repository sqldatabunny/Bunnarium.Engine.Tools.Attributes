
# Bunnarium Engine

How to use 💻✨🐇

***
## Installation 📀

**1.** Install any implementation layer for Bunnarium (e.g., Bunnarium.Implementation.MonoGame) and all of its dependencies. These dependencies will include Bunnarium.Engine.

**2.** Run the following from _[Your Project]/Program.cs_ (which should be a Console Application)

```cs
using Implementation = Bunnarium.Implementation.MonoGame;

public class Program {
    public static void Main(string[] args) {
        Implementation.Program.Run(args);
        }
    }
```

**3.** In your project, include a type that inherits from _Bunnarium.Engine.Core.Bootstrapping.GameEntryPoint_ and implements <ins>OnGameStart()</ins>.

```cs
using Bunnarium.Engine.Core.Bootstrapping;

public class MyGame : GameEntryPoint {
    public override void OnGameStart() {
        // Whatever you want your game to do ₍⑅ᐢ..ᐢ₎
        }
    }
```
***
## The BunnyECS Entity Component System 
### Creating and using GameObject Types ⚫

```cs
public class Lettuce : GameObject {
    protected override void OnDisable() {
        // mandatory method describingwhat you want to happen after the GameObject is unregistered from the ECS, if anything
        }

    protected override void OnEnable() {
        // mandatory method describing what you want to happen after the GameObject is created and registered with the ECS, if anything
        }

    protected override void Hop() {
        // optional virtual method for anything you want to this object to do on each frame. activated by setting the Hops property to true
        }
    }

public class BasketOfLettuce {

    Lettuce LettuceA { get; }
    Lettuce LettuceB { get; }
    Lettuce LettuceC { get; }
    
    public BasketOfLettuce() {

        var LettuceA = new Lettuce(); // disabled Lettuce instance that does not update each frame
        var LettuceB = new Lettuce() { Enabled = true }; // enabled Lettuce instance that is registered with the ECS
        var LettuceC = new Lettuce() { Enabled = true, Hops = true }; // enabled Lettuce instance that is registered with the ECS and that calls its Hop() function on each frame

        AssignComponents();
        }

    private void AssignComponents() {
        ref Transform2 transform = ref LettuceB.AddComponent<Transform2>();
        transform.Position += new Vector2<float>(1f, 0f); // move LettuceB one unit to the right

        // you can call the functions AddComponent<TComponent>(), HasComponent<TComponent>(), GetComponent<TComponent>(), and RemoveComponent<TComponent>() from GameObject instances

        // with Component types that support multiple instances of that component being assigned to the same GameObject, functions like CountComponents<TComponent>(),
        // GetComponents<TComponent>(), and RemoveComponents<TComponent>() are available
        
        }
    }
```
___
## Creating and Using Component Types 🔳

***Component Interface Explantions:*** Bunnarium Engine's component system defines uses several interfaces to describe Components and how they're used. These are:
+ + **IComponent&lt;TComponent>:** Interfaces applied to component types. 

+ **IComponentOperator&lt;TComponent>:** A modifier that wraps an IComponent&lt;TComponent> type.

+ **IComponentUnit:** An interface that both IComponent&lt;TComponent> and IComponentOperator&lt;TComponent> inherit from. An IComponentUnit is understood to be either an IComponent&lt;TComponent> or an IComponentOperator&lt;TComponent>, but this requirement is enforced at runtime and not at compiletime.

+ **IComponentSystemIsDirectCall:** Used to instruct the BunnyECS to call inheriting systems' OnHop delegates once per frame without passing any GameObject or Component system to that call's context.

+ **ISparseComponent&lt;TComponent, TSparseSetType&lt;TComponent>>:** The corollary of ITabularComponent, this interface is used to direct the ECS to store TComponents in SparseSets. ISparseComponents must be structs.

+ **ITabularComponent&lt;TComponent>:** The corollary of ISparseComponent, this interface is used to direct the ECS to store TComponents in archetype tables. ITabularComponents must be ***unmanaged*** types.

## Sparse vs. Tabular Components
Components ***MUST*** inherit from either ISparseComponent&lt;TComponent, TSparseSetType&lt;TComponent>> **OR** ITabularComponent&lt;TComponent>.

Sparse components are stored in sparse sets. Different types of sparse sets are provided, so you can tailor a sparse component type to one of them based on desired behavior. Sparse components must be structs, but they may contain reference types inside of them.

Tabular components, or archetypal components, are stored in the BunnyECS archetype system. Tabular components must be *unmanaged*, meaning that they cannot contain reference types at any level of nesting.

```cs
public struct Placeholder
    // both versions of this interface MUST be defined in the type definition
    : ISparseComponent<Placeholder, ISparseSet<Placeholder>>
    , ISparseComponent<Placeholder, SparseOneToOne<Placeholder>> {

    // one instance of the Placeholder component will be allowed per GameObject, and they will be stored in a sparse set
    }


public struct Placeholder
    // both versions of this interface MUST be defined in the type definition
    : ISparseComponent<Placeholder, ISparseSet<Placeholder>>
    , ISparseComponent<Placeholder, SparseOneToMany<Placeholder>> {

    // multiple instances of the Placeholder component will be allowed per GameObject, and they will be stored in a sparse set
    }


public struct Placeholder : ITabularComponent<Placeholder> {
    // multiple instances of the Placeholder component will be allowed per GameObject
    }
```

## Choosing a Component Type
If your component must contain a managed type, then you will need to use ISparseComponents. Any renderable component type, signified by a Rendered&lt;T>, must also be an ISparseComponent. For components unburdened by those two caviats, you should probably make them ITabularComponents for reasons that are explained in the [ComponentQuery performance](##⚠-A-warning-about-Component-Query-performance-⚠) section. To summarize these reasons, though, any system that interacts with a sparse component, 

___
## Creating ComponentSystem Types

There are two ways to create Component Systems. They are as follows:

### Method #1

The default method to create a Component System is to create a class that inherits from ComponentSystem and overrides the ComponentSystem(ComponentQuery&lt;TComponents> query) constructor.

The default method to create a Component System is to create a type that inherits from ComponentSystem. The ComponentSystem should implement a protected constructor that accepts a BunnyECS instance and that calls the base type's constructor that accepts a ComponentQuery&lt;TComponents> instance. This query can be created from the BunnyECS parameter. The ComponentQuery&lt;TComponents> expects the TComponents generic argument - this should be a ValueTuple containing the components that are to be retrieved via the query. See the Component Queries section below for more information.

The constructor should assign a value to the OnHop delegate. When processing the Component System will iterate through each GameObject that matches the Component Query and call OnHop(Span&lt;byte> data). The Span&lt;byte> is used to interact with the components associated with that entity.

```cs
// create a Component System that processes components on a by-entity basis.
public class ExampleSpriteAndTransformSystem : ComponentSystem {

    protected ExampleSpriteAndTransformSystem(BunnyECS parent)
        : base(parent.CreateQuery<(Transform2, Mutable<Sprite>)>()) {

        OnHop = (data) => {
            ref Transform2 position = ref GetComponent<Transform2>(data);
            Span<Sprite> sprites = GetComponents<Sprite>(data);
            for (int i = 0; i < sprites.Length; i++) {
                ref var sprite = ref sprites[i];
                sprite.Position = position.WorldPosition;
                sprite.Rotation = position.WorldOrientation;
                sprite.Scale = position.WorldScale;
                }
            };
        }
    }

// create a Component System that renders components
public class Example

```

### Method #2

A Component System can be created by creating class that inherits from ComponentSystem&lt;TComponentUnit, TComponent>, overrides the ComponentSystem(BunnyECS parent) constructor, and passes the *parent* parameter to the base constructor.

Like in Method #1, the OnHop delegate must be set in the constructor. Unlike in Method #1, however, OnHop provides access to a Span&lt;TComponent>, instead of a Span&lt;byte>. All instances of TComponent are stored in this Span, and you can iterate over them in OnHop itself rather than letting OnHop be a call that the ECS makes when iterating over GameObjects. This is faster, but there are requirements imposed on ComponentSystem&lt;TComponentUnit, TComponent> in exchange for this performance boost:

+ Only one type of component can be accessed using this method.
+ TComponent must be a struct that inherits from ISparseComponent&lt;TComponent, ISparseSet&lt;TComponent>>.
+ TComponentUnit must either wrap the same type as TComponent, or it must be TComponent exactly. For example, the generic argument pairs &lt;Mutable&lt;ComponentA>, ComponentA> and &lt;ComponentA, ComponentA> are valid, but &lt;Mutable&lt;ComponentA>, ComponentB> and &lt;ComponentA, ComponentB> are not.

```cs
public class PlaceholderSystem : ComponentSystem<Mutable<PlaceholderComponent>, PlaceholderComponent> {
   
    public PlaceholderSystem(BunnyECS parent) : base(parent) {
              
        OnHop = (data) => {
            for (var i = 0; i < data.Length; i++) {
                ref var placeholder = ref data[i];
                placeholder.Counter++;
                }
            }       
        }
    }
```

___
## Component Queries

Component Queries work via the following rules:
+ ComponentQuery&lt;TComponents> instances are created with the TComponent argument. TComponents, when passed in the ComponentSystem constructor's override, should be placed in a tuple.
+ The generic argument doesn't *need* to be a tuple - a single IComponentUnit-based type is also allowed, but it is unfavorable and there is a better way to create ComponentSystems that act against a single instance of a single type.
+ The query will return components for entities with a **matching** set of components.
+ This "match" is to be evaluated inclusively, meaning that entities must have *at least* the components
      described in the query, and an exact match isn't needed.
+ Multiple instances of the same component can be included in the query types for component types for which multiple instances are permitted per entity. For example, CreateQuery<(T1, T1, T2)>() will return components for all entities that have **at least** two T1 components and one T2 component.
+ If you want to make any state changes to components within its OnHop() call (more information below), then you will need to wrap that component type in the Rendered<T> Component Operator (more information also below).

## Component Operators
Component Operators wrap component type arguments. They may be used in ComponentQuery definitions or in special implementations of ComponentSystem that will be described later. They are:
+ Not&lt;TComponent>: Used in Component Queries to specify that the query should NOT return entities that have TComponents attached.
+ Mutable&lt;TComponent>: Used in Component Queries to signal to the ECS that the state of instances of that component type may be mutated while the ComponentSystem is processing. Any changes to the state of components that **aren't** wrapped in Mutable&lt;TComponent> will result in an error being thrown on ComponentSystem instantiation.
+ Rendered&lt;TComponent>: Used in special-case ComponentSystem<TComponentUnit, TComponent> definitions to signal to the ECS that instances of that component type will be rendered during its OnHop() call. Rendered&lt;TComponent> is not used in ComponentQueries.

## ⚠ A warning about Component Query performance ⚠
The existence of multiple component types impacts how Component Systems perform when using a Component Query to select qualifying GameObjects. The general rules apply:
+ When a Component System inherits from IComponentSystemIsDirectCall, OnHop is called once and no querying is performed. This is very fast, but it doesn't allow for components to be accessed by the OnHop action. When this is the case, all of the considerations below are inapplicable
+ When a Component System interacts with only tabular components, be it one or more of them, then query is very fast and cheap.
+ When a Component System interacts with both tabular and sparse components, it needs to check if each GameObject meets all relevant requirements with respect to the sparse components. This is performed on every frame, so there is a performance cost to these checks, but the cost is less severe when fewer sparse types are involved and when the tabular components specification narrows down the number of qualifying GameObjects before the sparse component checks are performed.
+ When a Component System interacts with only sparse components, each sparse set will need to be checked to make sure that a component is (or isn't, in the case of the Not&lt;T> operator) associated with each GameObject. BunnyECS will try to optimize the run through the sparse component sets invoked, but this process can be very slow depending on the number of sparse sets involved and how large the least-populated sparse set is. 

## The OnHop Action
 The ComponentSystem's constructor must define the OnHop(Span<byte>) action. This should be a method that accepts a Span<byte> that can be accessed via the GetComponent<T> and GetComponents<T> functions. It is in the OnHop method that you define what should be done with the components. In this context, OnHop is called for each entity.

 ℹ When calling GetComponent<T>, fetch a reference to it using the **ref** keyword. Failing to do so will mean that you'll assign a mere copy to your local variables.

___
## Rendering in a ComponentSystem

___
## ComponentSystem<TComponentUnit, TComponent>

___
## Component Types

## Alarms ⏱
Alarms are treated as components. Multiple Alarms can be assigned to a GameObject. They will be automatically removed from GameObjects when they expire, and they can be 

```cs
GameObject obj;
// initialize the GameObject
obj.AddComponent(new Alarm(
    duration: TimeInterval.FromSeconds(60),
    functionCall: () => Console.WriteLine("Ding!");
    ));
```

___
## RenderBunny 🎨
### Using RenderBunny
TODO
### Implementing RenderBunny
TODO
### Creating Custom RenderBunny Components
TODO
___
## Bunny Services 💬
BunnyServices are services that can run tasks asynchronously to the rest of the engine. They can be created by implementing Bunnarium.Engine.Core.Runtime.BunnyService&lt;T> and override Run();

```cs

public class ExampleService : BunnyService<ExampleService> {
    
    public ExampleService() {
        this.Enabled = true;
        this.TargetRunFrequency = System.TimeSpan.FromSeconds(60);
        }

    protected override void Run() {
        // what to do when the service runs
        }

    public override bool OutOfWork {
        get => false; // replace with a function that will determine whether the service doesn't need to run            
        }
    }

Because Bunny Services run as asynchronous Tasks, trying to access engine resources is frought with peril. You will need to lock accessed resources to avoid collisions.

```

___
## Self-Optimization using OptimizationManager

___
## Generic Math Support 🧮

### Floating-Point Precision
Many built-in types, particularly those in the Bunnarium.Engine.Maths namespace, use generics to define what floating-point precision is used. For example, Vector2&lt;T> may be either Vector2&lt;float> or Vector2&lt;double>. Most of these types expect the argument T to inherit from System.Numerics.IFloatingPointIeee754&lt;T>, meaning that float, double, and System.Half are legitimate generic parameters. However, some of these types will reject the use of System.Half, so use will normally be constrained to either float or double.

### Dimensional Abstraction

Bunnarium supports the creation of types that are dimension-agnostic - in other words, types that can be abstracted to work in both the second and third dimension.


```cs
using System.Numerics;
using Bunnarium.Engine.Maths.Primitives;
using Bunnarium.Engine.Maths.Geometry;
using Bunnarium.Engine.Tools.DataStructures;

public class DimensionAgnosticType<TMatrix, TBox, TRound, TLine, TDirection, TRotation, TVector, TNumeric>
where TMatrix : struct, IMatrix<Numeric>.ITransformMatrix<TMatrix, TVector, TDirection, TRotation>
where TBox : struct, IBox<TBox, TRound, TLine, TVector, TNumeric>
where TRound : struct, IRound<TRound, TBox, TLine, TVector, TNumeric>
where TLine : struct ILine<TLine, TVector, TNumeric>
where TDirection : struct, IDirection<TDirection, TRotation, TVector, TNumeric>
where TRotation : struct, IRotation<TRotation, TDirection, TVector, TNumeric>
where TVector : struct, IVector<TVector, TNumeric>
where TNumeric : unmanaged, IBinaryFloatingPointIeee754<TNumeric>, IMinMaxValue<TNumeric> {

    TVector Position { get; set; }
    
    TDirection Direction { get; set; }

    TNumeric Speed { get; set; }

    TBox Box { get; set; }

    public bool Foo() {
        // example of genreic code
        var nextPosition = Position + Direction.Vector * Speed;
        return Box.Contains(nextPosition);
        }
    }
```
```cs

static DimensionAgnosticType<Matrix2x3<float>, AABB2<float>, CirclePrimitive<float>, Line2<float>, Angle<float>, Angle<float>, Vector2<float>, float> SinglePrecision2DType { get; }

static DimensionAgnosticType<Matrix3x4<double>, AABB3<double>, SpherePrimitive<double>, Line3<double>, Direction<double>, Quaternion<double>, Vector3<double>, double> DoublePrecision3DType { get; }

```

I know it looks scary, but you can apply using or global using statements to make the rest of your codebase cleaning!

```cs

global using My2DType = Bunnarium.Engine.DimensionAgnosticType<Bunnarium.Engine.Maths.Primitives.Matrix2x3<float>, Bunnarium.Engine.Maths.Geometry.AABB2<float>, Bunnarium.Engine.Maths.Geometry.CirlcePrimitive<float>, Bunnarium.Engine.Maths.Geometry.Line2<float>, Bunnarium.Engine.Maths.Primitives.Angle<float>, Bunnarium.Engine.Maths.Primitives.Angle<float>, Bunnarium.Engine.Maths.Primitives.Vector2<float>, float>.


global using My3DType = Bunnarium.Engine.DimensionAgnosticType<Bunnarium.Engine.Maths.Primitives.Matrix3x4<double>, Bunnarium.Engine.Maths.Geometry.AABB3<double>, Bunnarium.Engine.Maths.Geometry.SpherePrimitive<double>, Bunnarium.Engine.Maths.Geometry.Line3<double>, Bunnarium.Engine.Maths.Primitives.Direction<double>, Bunnarium.Engine.Maths.Primitives.Quaternion<double>, Bunnarium.Engine.Maths.Primitives.Vector3<double>, double>.

/* ... */

static My2DType SinglePrecision2DType { get; }
static My3DType DoublePrecision3DType { get; }

```

Don't let the large type signatures of these generics intimidate you - they're here to help!

___
## Creating a Framework Implementation 🧵
Bunnarium Engine is intended to work without necessarily depending on any particular frameworks for operations such as input scanning, windowing, or rendering with a graphics API. As such, a variety of frameworks should be usable with Bunnarium—so long as an implementation layer is built for the desired framework(s). To do so, you will need to create a new project. This project should have all Bunnarium.Engine modules as registered dependencies and should be the module that your game depends on to run. For the following explanation, we'll use "Bunnarium.Implementation.FluffleFramework" (a fiction created for our example).

The "FluffleFramework" will foremost need to implement the Bunnarium.Engine.Core.Bootstrapping.EngineBootstrapper type.

```cs
internal class FluffleFrameworkBootstrapper : EngineBootstrapper {   
    internal FluffleFrameworkBootstrapper() {
        // assignment of abstract members  
        } 
    // overrides of abstract members  
    }
```

EngineBootstrapper provides all of the requirements that your framework needs to fulfill to make Bunnarium Engine function. Many of the requirements are properties, Action<\*>s, or Func<\*>s that will need to provide Fluffle-specific implementations of other abstract Bunnarium types. For example:

```cs
internal class FluffleFrameworkBootstrapper : EngineBootstrapper {
    
    public override IControlPanel ControlPanel { get; }
    public override Func<GraphicsLimitations { get; }
    public override Action InitializeGraphicsDevice { get; }
    
    // etc...
    
    internal FluffleFrameworkBootstrapper() {
    
        ControlPanel = new PlatformControlPanel();      
        GraphicsLimitations = (() => new PlatformGraphicsLimitations);
        InitializeGraphicsDevice = SomePlatformType.InitilizeGraphics;
        
        // etc...      
        }
    }
```

Some of these types may be very complex, and implementing Bunnarium on your chosen frameworks may not be straightforward.
Good luck ૮˶ᵔᵕᵔ7