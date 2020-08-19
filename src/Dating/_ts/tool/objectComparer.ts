export class ObjectComparer {
    public areDeepEqual(alpha: any, beta: any): boolean {
        let chainA = new Array<object>();
        let chainB = new Array<object>();
        if (!this.objectsAreEqual(alpha, beta, chainA, chainB)) {
            return false;
        }

        chainA = new Array<object>();
        chainB = new Array<object>();
        if (!this.objectsAreEqual(beta, alpha, chainA, chainB)) {
            return false;
        }

        return true;
    }

    private objectsAreEqual(alpha: any, beta: any, chainA: Array<object>, chainB: Array<object>): boolean {
        var p;

        // NaN === NaN returns false
        // and isNaN(undefined) returns true
        if (typeof (alpha) === 'number'
            && typeof (beta) === 'number'
            && isNaN(alpha)
            && isNaN(beta)) {
            return true;
        }

        // Compare primitives and functions.     
        // Check if both arguments link to the same object.
        // Especially useful on the step where we compare prototypes
        if (alpha === beta) {
            return true;
        }

        // Works in case when functions are created in constructor.
        // Comparing dates is a common scenario. Another built-ins?
        // We can even handle functions passed across iframes
        if ((typeof (alpha) === 'function' && typeof (beta) === 'function')
            || (alpha instanceof Date && beta instanceof Date)
            || (alpha instanceof RegExp && beta instanceof RegExp)
            || (alpha instanceof String && beta instanceof String)
            || (alpha instanceof Number && beta instanceof Number)) {
            return alpha.toString() === beta.toString();
        }

        // At last checking prototypes as good as we can
        if (!(alpha instanceof Object && beta instanceof Object)) {
            return false;
        }

        if (alpha.isPrototypeOf(beta) || beta.isPrototypeOf(alpha)) {
            return false;
        }

        if (alpha.constructor !== beta.constructor) {
            return false;
        }

        if (alpha.prototype !== beta.prototype) {
            return false;
        }

        // Check for infinitive linking loops
        if (chainA.indexOf(alpha) > -1 || chainB.indexOf(beta) > -1) {
            return false;
        }

        // Quick checking of one object being a subset of another.
        // todo: cache the structure of arguments[0] for performance
        for (p in beta) {
            if (beta.hasOwnProperty(p) !== alpha.hasOwnProperty(p)) {
                return false;
            }
            else if (typeof beta[p] !== typeof alpha[p]) {
                return false;
            }
        }

        for (p in alpha) {
            if (beta.hasOwnProperty(p) !== alpha.hasOwnProperty(p)) {
                return false;
            }
            else if (typeof beta[p] !== typeof alpha[p]) {
                return false;
            }

            switch (typeof (alpha[p])) {
                case 'object':
                case 'function':

                    chainA.push(alpha);
                    chainB.push(beta);

                    if (!this.objectsAreEqual(alpha[p], beta[p], chainA, chainB)) {
                        return false;
                    }

                    chainA.pop();
                    chainB.pop();
                    break;

                default:
                    if (alpha[p] !== beta[p]) {
                        return false;
                    }
                    break;
            }
        }

        return true;
    }
}