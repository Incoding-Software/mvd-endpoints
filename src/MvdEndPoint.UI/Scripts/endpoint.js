!function (e) { var t, n, r, l, o; return o = ["object", "array", "number", "string", "boolean", "null"], r = function () { function t(e) { null == e && (e = {}), this.options = e } return t.prototype.htmlEncode = function (e) { return null !== e ? e.toString().replace(/&/g, "&amp;").replace(/"/g, "&quot;").replace(/</g, "&lt;").replace(/>/g, "&gt;") : "" }, t.prototype.jsString = function (e) { return e = JSON.stringify(e).slice(1, -1), this.htmlEncode(e) }, t.prototype.decorateWithSpan = function (e, t) { return '<span class="' + t + '">' + this.htmlEncode(e) + "</span>" }, t.prototype.valueToHTML = function (t, n) { var r; if (null == n && (n = 0), r = Object.prototype.toString.call(t).match(/\s(.+)]/)[1].toLowerCase(), this.options.strict && !e.inArray(r, o)) throw new Error("" + r + " is not a valid JSON value type"); return this["" + r + "ToHTML"].call(this, t, n) }, t.prototype.nullToHTML = function (e) { return this.decorateWithSpan("null", "null") }, t.prototype.undefinedToHTML = function () { return this.decorateWithSpan("undefined", "undefined") }, t.prototype.numberToHTML = function (e) { return this.decorateWithSpan(e, "num") }, t.prototype.stringToHTML = function (e) { var t, n; return /^(http|https|file):\/\/[^\s]+$/i.test(e) ? '<a href="' + this.htmlEncode(e) + '"><span class="q">"</span>' + this.jsString(e) + '<span class="q">"</span></a>' : (t = "", e = this.jsString(e), this.options.nl2br && (n = /([^>\\r\\n]?)(\\r\\n|\\n\\r|\\r|\\n)/g, n.test(e) && (t = " multiline", e = (e + "").replace(n, "$1<br />"))), '<span class="string' + t + '">"' + e + '"</span>') }, t.prototype.booleanToHTML = function (e) { return this.decorateWithSpan(e, "bool") }, t.prototype.arrayToHTML = function (e, t) { var n, r, l, o, i, s, a, p; for (null == t && (t = 0), r = !1, i = "", o = e.length, l = a = 0, p = e.length; p > a; l = ++a) s = e[l], r = !0, i += "<li>" + this.valueToHTML(s, t + 1), o > 1 && (i += ","), i += "</li>", o--; return r ? (n = 0 === t ? "" : " collapsible", '[<ul class="array level' + t + n + '">' + i + "</ul>]") : "[ ]" }, t.prototype.objectToHTML = function (e, t) { var n, r, l, o, i, s, a; null == t && (t = 0), r = !1, i = "", o = 0; for (s in e) o++; for (s in e) a = e[s], r = !0, l = this.options.escape ? this.jsString(s) : s, i += '<li><a class="prop" href="javascript:;"><span class="q">"</span>' + l + '<span class="q">"</span></a>: ' + this.valueToHTML(a, t + 1), o > 1 && (i += ","), i += "</li>", o--; return r ? (n = 0 === t ? "" : " collapsible", '{<ul class="obj level' + t + n + '">' + i + "</ul>}") : "{ }" }, t.prototype.jsonToHTML = function (e) { return '<div class="jsonview">' + this.valueToHTML(e) + "</div>" }, t }(), "undefined" != typeof module && null !== module && (module.exports = r), n = function () { function e() { } return e.bindEvent = function (e, t) { var n; return e.firstChild.addEventListener("click", function (e) { return function (n) { return e.toggle(n.target.parentNode.firstChild, t) } }(this)), n = document.createElement("div"), n.className = "collapser", n.innerHTML = t.collapsed ? "+" : "-", n.addEventListener("click", function (e) { return function (n) { return e.toggle(n.target, t) } }(this)), e.insertBefore(n, e.firstChild), t.collapsed ? this.collapse(n) : void 0 }, e.expand = function (e) { var t, n; return n = this.collapseTarget(e), "" !== n.style.display ? (t = n.parentNode.getElementsByClassName("ellipsis")[0], n.parentNode.removeChild(t), n.style.display = "", e.innerHTML = "-") : void 0 }, e.collapse = function (e) { var t, n; return n = this.collapseTarget(e), "none" !== n.style.display ? (n.style.display = "none", t = document.createElement("span"), t.className = "ellipsis", t.innerHTML = " &hellip; ", n.parentNode.insertBefore(t, n), e.innerHTML = "+") : void 0 }, e.toggle = function (e, t) { var n, r, l, o, i, s; if (null == t && (t = {}), l = this.collapseTarget(e), n = "none" === l.style.display ? "expand" : "collapse", t.recursive_collapser) { for (r = e.parentNode.getElementsByClassName("collapser"), s = [], o = 0, i = r.length; i > o; o++) e = r[o], s.push(this[n](e)); return s } return this[n](e) }, e.collapseTarget = function (e) { var t, n; return n = e.parentNode.getElementsByClassName("collapsible"), n.length ? t = n[0] : void 0 }, e }(), t = e, l = { collapse: function (e) { return "-" === e.innerHTML ? n.collapse(e) : void 0 }, expand: function (e) { return "+" === e.innerHTML ? n.expand(e) : void 0 }, toggle: function (e) { return n.toggle(e) } }, t.fn.JSONView = function () { var e, o, i, s, a, p, c; return e = arguments, null != l[e[0]] ? (a = e[0], this.each(function () { var n, r; return n = t(this), null != e[1] ? (r = e[1], n.find(".jsonview .collapsible.level" + r).siblings(".collapser").each(function () { return l[a](this) })) : n.find(".jsonview > ul > li .collapsible").siblings(".collapser").each(function () { return l[a](this) }) })) : (s = e[0], p = e[1] || {}, o = { collapsed: !1, nl2br: !1, recursive_collapser: !1, escape: !0, strict: !1 }, p = t.extend(o, p), i = new r(p), "[object String]" === Object.prototype.toString.call(s) && (s = JSON.parse(s)), c = i.jsonToHTML(s), this.each(function () { var e, r, l, o, i, s; for (e = t(this), e.html(c), l = e[0].getElementsByClassName("collapsible"), s = [], o = 0, i = l.length; i > o; o++) r = l[o], "LI" === r.parentNode.nodeName ? s.push(n.bindEvent(r.parentNode, p)) : s.push(void 0); return s })) } }(jQuery);


$(function () {
    hljs.initHighlightingOnLoad();
    $('textarea').on('tbwblur', function (e) {
        $(e.currentTarget).trigger('blur');
    });
});

function DataToString(target, json) {
    $(target).JSONView(JSON.stringify(json));
}



/* NProgress, (c) 2013, 2014 Rico Sta. Cruz - http://ricostacruz.com/nprogress
 * @license MIT */

; (function (root, factory) {

    if (typeof define === 'function' && define.amd) {
        define(factory);
    } else if (typeof exports === 'object') {
        module.exports = factory();
    } else {
        root.NProgress = factory();
    }

})(this, function () {
    var NProgress = {};

    NProgress.version = '0.2.0';

    var Settings = NProgress.settings = {
        minimum: 0.2,
        easing: 'ease',
        positionUsing: '',
        speed: 200,
        trickle: true,
        trickleRate: 0.02,
        trickleSpeed: 800,
        showSpinner: true,
        barSelector: '[role="bar"]',
        spinnerSelector: '[role="spinner"]',
        parent: 'body',
        template: '<div class="bar" role="bar"><div class="peg"></div></div><div class="spinner" role="spinner"><div class="spinner-icon"></div></div>'
    };

    /**
     * Updates configuration.
     *
     *     NProgress.configure({
     *       minimum: 0.1
     *     });
     */
    NProgress.configure = function (options) {
        var key, value;
        for (key in options) {
            value = options[key];
            if (value !== undefined && options.hasOwnProperty(key)) Settings[key] = value;
        }

        return this;
    };

    /**
     * Last number.
     */

    NProgress.status = null;

    /**
     * Sets the progress bar status, where `n` is a number from `0.0` to `1.0`.
     *
     *     NProgress.set(0.4);
     *     NProgress.set(1.0);
     */

    NProgress.set = function (n) {
        var started = NProgress.isStarted();

        n = clamp(n, Settings.minimum, 1);
        NProgress.status = (n === 1 ? null : n);

        var progress = NProgress.render(!started),
            bar = progress.querySelector(Settings.barSelector),
            speed = Settings.speed,
            ease = Settings.easing;

        progress.offsetWidth; /* Repaint */

        queue(function (next) {
            // Set positionUsing if it hasn't already been set
            if (Settings.positionUsing === '') Settings.positionUsing = NProgress.getPositioningCSS();

            // Add transition
            css(bar, barPositionCSS(n, speed, ease));

            if (n === 1) {
                // Fade out
                css(progress, {
                    transition: 'none',
                    opacity: 1
                });
                progress.offsetWidth; /* Repaint */

                setTimeout(function () {
                    css(progress, {
                        transition: 'all ' + speed + 'ms linear',
                        opacity: 0
                    });
                    setTimeout(function () {
                        NProgress.remove();
                        next();
                    }, speed);
                }, speed);
            } else {
                setTimeout(next, speed);
            }
        });

        return this;
    };

    NProgress.isStarted = function () {
        return typeof NProgress.status === 'number';
    };

    /**
     * Shows the progress bar.
     * This is the same as setting the status to 0%, except that it doesn't go backwards.
     *
     *     NProgress.start();
     *
     */
    NProgress.start = function () {
        if (!NProgress.status) NProgress.set(0);

        var work = function () {
            setTimeout(function () {
                if (!NProgress.status) return;
                NProgress.trickle();
                work();
            }, Settings.trickleSpeed);
        };

        if (Settings.trickle) work();

        return this;
    };

    /**
     * Hides the progress bar.
     * This is the *sort of* the same as setting the status to 100%, with the
     * difference being `done()` makes some placebo effect of some realistic motion.
     *
     *     NProgress.done();
     *
     * If `true` is passed, it will show the progress bar even if its hidden.
     *
     *     NProgress.done(true);
     */

    NProgress.done = function (force) {
        if (!force && !NProgress.status) return this;

        return NProgress.inc(0.3 + 0.5 * Math.random()).set(1);
    };

    /**
     * Increments by a random amount.
     */

    NProgress.inc = function (amount) {
        var n = NProgress.status;

        if (!n) {
            return NProgress.start();
        } else {
            if (typeof amount !== 'number') {
                amount = (1 - n) * clamp(Math.random() * n, 0.1, 0.95);
            }

            n = clamp(n + amount, 0, 0.994);
            return NProgress.set(n);
        }
    };

    NProgress.trickle = function () {
        return NProgress.inc(Math.random() * Settings.trickleRate);
    };

    /**
     * Waits for all supplied jQuery promises and
     * increases the progress as the promises resolve.
     *
     * @param $promise jQUery Promise
     */
    (function () {
        var initial = 0, current = 0;

        NProgress.promise = function ($promise) {
            if (!$promise || $promise.state() === "resolved") {
                return this;
            }

            if (current === 0) {
                NProgress.start();
            }

            initial++;
            current++;

            $promise.always(function () {
                current--;
                if (current === 0) {
                    initial = 0;
                    NProgress.done();
                } else {
                    NProgress.set((initial - current) / initial);
                }
            });

            return this;
        };

    })();

    /**
     * (Internal) renders the progress bar markup based on the `template`
     * setting.
     */

    NProgress.render = function (fromStart) {
        if (NProgress.isRendered()) return document.getElementById('nprogress');

        addClass(document.documentElement, 'nprogress-busy');

        var progress = document.createElement('div');
        progress.id = 'nprogress';
        progress.innerHTML = Settings.template;

        var bar = progress.querySelector(Settings.barSelector),
            perc = fromStart ? '-100' : toBarPerc(NProgress.status || 0),
            parent = document.querySelector(Settings.parent),
            spinner;

        css(bar, {
            transition: 'all 0 linear',
            transform: 'translate3d(' + perc + '%,0,0)'
        });

        if (!Settings.showSpinner) {
            spinner = progress.querySelector(Settings.spinnerSelector);
            spinner && removeElement(spinner);
        }

        if (parent != document.body) {
            addClass(parent, 'nprogress-custom-parent');
        }

        parent.appendChild(progress);
        return progress;
    };

    /**
     * Removes the element. Opposite of render().
     */

    NProgress.remove = function () {
        removeClass(document.documentElement, 'nprogress-busy');
        removeClass(document.querySelector(Settings.parent), 'nprogress-custom-parent');
        var progress = document.getElementById('nprogress');
        progress && removeElement(progress);
    };

    /**
     * Checks if the progress bar is rendered.
     */

    NProgress.isRendered = function () {
        return !!document.getElementById('nprogress');
    };

    /**
     * Determine which positioning CSS rule to use.
     */

    NProgress.getPositioningCSS = function () {
        // Sniff on document.body.style
        var bodyStyle = document.body.style;

        // Sniff prefixes
        var vendorPrefix = ('WebkitTransform' in bodyStyle) ? 'Webkit' :
                           ('MozTransform' in bodyStyle) ? 'Moz' :
                           ('msTransform' in bodyStyle) ? 'ms' :
                           ('OTransform' in bodyStyle) ? 'O' : '';

        if (vendorPrefix + 'Perspective' in bodyStyle) {
            // Modern browsers with 3D support, e.g. Webkit, IE10
            return 'translate3d';
        } else if (vendorPrefix + 'Transform' in bodyStyle) {
            // Browsers without 3D support, e.g. IE9
            return 'translate';
        } else {
            // Browsers without translate() support, e.g. IE7-8
            return 'margin';
        }
    };

    /**
     * Helpers
     */

    function clamp(n, min, max) {
        if (n < min) return min;
        if (n > max) return max;
        return n;
    }

    /**
     * (Internal) converts a percentage (`0..1`) to a bar translateX
     * percentage (`-100%..0%`).
     */

    function toBarPerc(n) {
        return (-1 + n) * 100;
    }


    /**
     * (Internal) returns the correct CSS for changing the bar's
     * position given an n percentage, and speed and ease from Settings
     */

    function barPositionCSS(n, speed, ease) {
        var barCSS;

        if (Settings.positionUsing === 'translate3d') {
            barCSS = { transform: 'translate3d(' + toBarPerc(n) + '%,0,0)' };
        } else if (Settings.positionUsing === 'translate') {
            barCSS = { transform: 'translate(' + toBarPerc(n) + '%,0)' };
        } else {
            barCSS = { 'margin-left': toBarPerc(n) + '%' };
        }

        barCSS.transition = 'all ' + speed + 'ms ' + ease;

        return barCSS;
    }

    /**
     * (Internal) Queues a function to be executed.
     */

    var queue = (function () {
        var pending = [];

        function next() {
            var fn = pending.shift();
            if (fn) {
                fn(next);
            }
        }

        return function (fn) {
            pending.push(fn);
            if (pending.length == 1) next();
        };
    })();

    /**
     * (Internal) Applies css properties to an element, similar to the jQuery 
     * css method.
     *
     * While this helper does assist with vendor prefixed property names, it 
     * does not perform any manipulation of values prior to setting styles.
     */

    var css = (function () {
        var cssPrefixes = ['Webkit', 'O', 'Moz', 'ms'],
            cssProps = {};

        function camelCase(string) {
            return string.replace(/^-ms-/, 'ms-').replace(/-([\da-z])/gi, function (match, letter) {
                return letter.toUpperCase();
            });
        }

        function getVendorProp(name) {
            var style = document.body.style;
            if (name in style) return name;

            var i = cssPrefixes.length,
                capName = name.charAt(0).toUpperCase() + name.slice(1),
                vendorName;
            while (i--) {
                vendorName = cssPrefixes[i] + capName;
                if (vendorName in style) return vendorName;
            }

            return name;
        }

        function getStyleProp(name) {
            name = camelCase(name);
            return cssProps[name] || (cssProps[name] = getVendorProp(name));
        }

        function applyCss(element, prop, value) {
            prop = getStyleProp(prop);
            element.style[prop] = value;
        }

        return function (element, properties) {
            var args = arguments,
                prop,
                value;

            if (args.length == 2) {
                for (prop in properties) {
                    value = properties[prop];
                    if (value !== undefined && properties.hasOwnProperty(prop)) applyCss(element, prop, value);
                }
            } else {
                applyCss(element, args[1], args[2]);
            }
        }
    })();

    /**
     * (Internal) Determines if an element or space separated list of class names contains a class name.
     */

    function hasClass(element, name) {
        var list = typeof element == 'string' ? element : classList(element);
        return list.indexOf(' ' + name + ' ') >= 0;
    }

    /**
     * (Internal) Adds a class to an element.
     */

    function addClass(element, name) {
        var oldList = classList(element),
            newList = oldList + name;

        if (hasClass(oldList, name)) return;

        // Trim the opening space.
        element.className = newList.substring(1);
    }

    /**
     * (Internal) Removes a class from an element.
     */

    function removeClass(element, name) {
        var oldList = classList(element),
            newList;

        if (!hasClass(element, name)) return;

        // Replace the class name.
        newList = oldList.replace(' ' + name + ' ', ' ');

        // Trim the opening and closing spaces.
        element.className = newList.substring(1, newList.length - 1);
    }

    /**
     * (Internal) Gets a space separated list of the class names on the element. 
     * The list is wrapped with a single space on each end to facilitate finding 
     * matches within the list.
     */

    function classList(element) {
        return (' ' + (element.className || '') + ' ').replace(/\s+/gi, ' ');
    }

    /**
     * (Internal) Removes an element from the DOM.
     */

    function removeElement(element) {
        element && element.parentNode && element.parentNode.removeChild(element);
    }

    return NProgress;
});
