var test = { key: 1, key2: 2 };
test[Symbol.iterator] = function* () {
    for (var i = 0; i < 5; i++)
        yield i;
}

var test2 = {};
test2[Symbol.iterator] = function* () {
    for (var i = yield 'begin' in ((yield 'source'), test))
        yield i;

    switch (1) {
        case yield 'switch0':
        case 1:
        case yield 'swithc1':
            break;
    }

    i = 0;
    do {
        if (!(i % 2))
            yield i;
    }
    while (i++ <= 10);

    try {
        try {
            yield 'try';
            throw null;
        } catch (e) {
            yield 'catch';
        } finally {
            yield 'finally';
        }
    } catch (e)
    { }

    var a = 1,
        b = yield (c || 'b'),
        c = 'c';
    if (c != 'c')
        throw 'c != "c"';

    with ((yield 'with') || { iw: 'insideWith' }) {
        yield iw;
    }
}

for (var i of test2)
    console.log(i);