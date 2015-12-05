class A {
    constructor() {
        this.text = new.target.name;
    }
}

class B extends A { constructor() { super(); } }

var a = new A().text; // logs "A"
var b = new B().text; // logs "B"

if (a != "A")
    console.log("new.target works incorrectly");
if (b != "B")
    console.log("new.target works incorrectly");