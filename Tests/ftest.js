class BaseClass {
    sayHello() {
        console.log("Hello, i'm Base");
    }
}

class DerivedClass extends BaseClass {
    sayHello() {
        console.log("Hello, i'm Derived");
        super.sayHello();
    }
}

var instance = new DerivedClass();
instance.sayHello();