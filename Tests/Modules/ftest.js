
import { Forms as forms } from "/clr/System/Windows"
import * as mm from "mymodule";
import defaultFunc, { v, changeV as setV } from "mymodule"

console.log(v);
setV("new value");
console.log(v);

mm.changeV("yet another value");
console.log(v);

console.log(mm.default);
console.log(defaultFunc);

console.log(mm.e);
console.log(mm.E);
console.log(mm.default);
console.log(mm.Pi);