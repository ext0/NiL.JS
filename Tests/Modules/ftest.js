
console.log(Error.constructor == Function.constructor);
console.log(Error.constructor().__proto__ == Function.prototype);
console.log(Object.call(Error).__proto__ == Object.prototype);