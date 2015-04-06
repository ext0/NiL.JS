/// Copyright (c) 2012 Ecma International.  All rights reserved. 
/**
 * @path ch15/15.2/15.2.3/15.2.3.14/15.2.3.14-5-10.js
 * @description Object.keys - inherted enumerable accessor property that is over-ridden by non-enumerable own accessor property is not defined in returned array
 */


function testcase() {
        var proto = {};
        Object.defineProperty(proto, "prop", {
            get: function () { },
            enumerable: true,
            configurable: true
        });
        var Con = function () { };
        Con.prototype = proto;

        var obj = new Con();
        Object.defineProperty(obj, "prop", {
            get: function () { },
            enumerable: false,
            configurable: true
        });

        var arr = Object.keys(obj);

        for (var p in arr) {
            if (arr[p] === "prop") {
                return false;
            }
        }

        return true;
    }
runTestCase(testcase);