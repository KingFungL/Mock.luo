﻿! function (e) {
        e.jqx.jqxWidget("jqxTreeGrid", "jqxDataTable", {}), e.extend(e.jqx._jqxTreeGrid.prototype, {
            defineInstance: function () {
                this.base && (this.base.treeGrid = this, this.base.exportSettings = {
                    recordsInView: !1,
                    columnsHeader: !0,
                    hiddenColumns: !1,
                    serverURL: null,
                    characterSet: null,
                    collapsedRecords: !1,
                    fileName: "jqxTreeGrid"
                });
                var r = {
                    pageSizeMode: "default",
                    checkboxes: !1,
                    hierarchicalCheckboxes: !1,
                    icons: !1,
                    showSubAggregates: !1,
                    rowDetailsRenderer: null,
                    virtualModeCreateRecords: null,
                    virtualModeRecordCreating: null,
                    loadingFailed: !1
                };
                return this === e.jqx._jqxTreeGrid.prototype ? r : (e.extend(!0, this, r), r)
            },
            createInstance: function (e) {
                this.theme = this.base.theme
            },
            deleteRow: function (e) {
                this.base.deleterowbykey(e)
            },
            updateRow: function (e, r) {
                this.base.updaterowbykey(e, r)
            },
            setCellValue: function (e, r, t) {
                this.base.setCellValueByKey(e, r, t)
            },
            getCellValue: function (e, r) {
                return this.base.getCellValueByKey(e, r)
            },
            lockRow: function (e) {
                this.base.lockrowbykey(e)
            },
            unlockRow: function (e) {
                this.base.unlockrowbykey(e)
            },
            selectRow: function (e) {
                this.base.selectrowbykey(e)
            },
            unselectRow: function (e) {
                this.base.unselectrowbykey(e)
            },
            ensureRowVisible: function (e) {
                this.base.ensurerowvisiblebykey(e)
            },
            beginCellEdit: function (e, r) {
                var t = this.base,
                    i = t.getColumn(r);
                t.beginroweditbykey(e, i)
            },
            beginRowEdit: function (e) {
                this.base.beginroweditbykey(e)
            },
            endCellEdit: function (e, r, t) {
                this.base.endroweditbykey(e, t)
            },
            endRowEdit: function (e, r) {
                this.base.endroweditbykey(e, r)
            },
            _showLoadElement: function () {
                var r = this.base;
                "block" == r.host.css("display") && r.autoShowLoadElement && (e(r.dataloadelement).css("visibility", "visible"), e(r.dataloadelement).css("display", "block"), r.dataloadelement.width(r.host.width()), r.dataloadelement.height(r.host.height()))
            },
            _hideLoadElement: function () {
                var r = this.base;
                "block" == r.host.css("display") && r.autoShowLoadElement && (e(r.dataloadelement).css("visibility", "hidden"), e(r.dataloadelement).css("display", "none"), r.dataloadelement.width(r.host.width()), r.dataloadelement.height(r.host.height()))
            },
            getKey: function (e) {
                if (e) return e.uid
            },
            getRows: function () {
                var e = this.base;
                return e.source.hierarchy && 0 != e.source.hierarchy.length ? e.source.hierarchy : e.source.records
            },
            getCheckedRows: function () {
                var r = this.base,
                    t = r._names(),
                    i = new Array,
                    o = function (a, l) {
                        if (l) for (var s = 0; s < l.length; s++) if (l[s]) {
                            var n = e.extend({}, l[s]),
                                d = r.rowinfo[l[s].uid];
                            d && d[t.checked] ? a.push(n) : n[t.checked] && a.push(n), o(i, l[s].records)
                        }
                    };
                return o(i, r.dataViewRecords), i
            },
            getRow: function (e) {
                var r = this.base,
                    t = r.source.records;
                if (r.source.hierarchy) {
                    var i = function (r) {
                        for (var t = 0; t < r.length; t++) if (r[t]) {
                            if (r[t].uid == e) return r[t];
                            if (r[t].records) {
                                var o = i(r[t].records);
                                if (o) return o
                            }
                        }
                    };
                    return i(r.source.hierarchy)
                }
                for (var o = 0; o < t.length; o++) if (t[o] && t[o].uid == e) return t[o]
            },
            _renderrows: function () {
                var r = this.base,
                    t = this;
                if (!r._loading && !r._updating) {
                    var i = r._names();
                    if (0 === r.source.hierarchy.length && !r.loadingFailed && this.virtualModeCreateRecords) {
                        r._loading = !0, this.virtualModeCreateRecords(null, function (e) {
                            if (!1 === e || e && 0 == e.length) return r._loading = !1, r.loadingFailed = !0, r.source.hierarchy = new Array, t._hideLoadElement(), r._renderrows(), r._updateScrollbars(), void r._arrange();
                            for (var i = 0; i < e.length; i++) e[i].level = 0, t.virtualModeRecordCreating(e[i]), r.rowsByKey[e[i].uid] = e[i];
                            r.source.hierarchy = e, r.source._source.hierarchy || (r.source._source.hierarchy = {}), r._loading = !1, t._hideLoadElement(), r._renderrows(), r._updateScrollbars(), r._arrange()
                        }), this._showLoadElement()
                    }
                    r.rendering && r.rendering();
                    var o = 0;
                    r.table[0].rows = new Array;
                    C = r.toTP("jqx-cell") + " " + r.toTP("jqx-widget-content") + " " + r.toTP("jqx-item");
                    r.rtl && (C += " " + r.toTP("jqx-cell-rtl"));
                    var a = r.columns.records.length,
                        l = e.jqx.browser.msie && e.jqx.browser.version < 8;
                    l && r.host.attr("hideFocus", "true");
                    var s = new Array,
                        n = function (e, t) {
                            for (var o = 0; o < e.length; o++) {
                                var a = e[o];
                                if (a) {
                                    var l = r.rowinfo[a.uid] ? r.rowinfo[a.uid].expanded : a.expanded;
                                    if (0 == r.dataview.filters.length && (a._visible = !0), !1 !== a._visible) if (l || a[i.leaf]) {
                                        if (t.push(a), a.records && a.records.length > 0) for (var s = n(a.records, new Array), d = 0; d < s.length; d++) t.push(s[d])
                                    } else t.push(a)
                                }
                            }
                            return t
                        },
                        d = 0 === r.source.hierarchy.length ? r.source.records : r.source.hierarchy;
                    if (d = r.dataview.evaluate(d), r.dataViewRecords = d, this.showSubAggregates) {
                        var c = function (r, t) {
                            0 != r && t.length > 0 && (t[t.length - 1] ? t[t.length - 1].aggregate || t.push({
                                _visible: !0,
                                level: r,
                                siblings: t,
                                aggregate: !0,
                                leaf: !0
                            }) : e.jqx.browser.msie && e.jqx.browser.version < 9 && t[t.length - 2] && (t[t.length - 2].aggregate || t.push({
                                _visible: !0,
                                level: r,
                                siblings: t,
                                aggregate: !0,
                                leaf: !0
                            })));
                            for (var i = 0; i < t.length; i++) t[i] && t[i].records && c(r + 1, t[i].records)
                        };
                        c(0, d)
                    }
                    var h = function (e) {
                        for (var t = 0, o = new Array, a = 0; a < e.length; a++) {
                            var l = e[a];
                            if (0 == l[i.level] && t++ , t > r.dataview.pagesize * r.dataview.pagenum && t <= r.dataview.pagesize * r.dataview.pagenum + r.dataview.pagesize && o.push(l), t > r.dataview.pagesize * r.dataview.pagenum + r.dataview.pagesize) break
                        }
                        return o
                    };
                    if (0 === r.source.hierarchy.length) {
                        if ("all" == r.dataview.pagesize || !r.pageable || r.serverProcessing) {
                            w = d;
                            if (r.pageable && r.serverProcessing && d.length > r.dataview.pagesize) w = d.slice(r.dataview.pagesize * r.dataview.pagenum, r.dataview.pagesize * r.dataview.pagenum + r.dataview.pagesize)
                        } else w = d.slice(r.dataview.pagesize * r.dataview.pagenum, r.dataview.pagesize * r.dataview.pagenum + r.dataview.pagesize);
                        s = w
                    } else {
                        d = n.call(r, d, new Array);
                        if ("all" != r.dataview.pagesize && r.pageable) {
                            w = d.slice(r.dataview.pagesize * r.dataview.pagenum, r.dataview.pagesize * r.dataview.pagenum + r.dataview.pagesize);
                            "root" == this.pageSizeMode && (w = h(d))
                        } else w = d;
                        var s = w,
                            g = r.dataview.pagenum;
                        if (r.updatepagerdetails(), r.dataview.pagenum != g) {
                            if ("all" != r.dataview.pagesize && r.pageable) {
                                w = d.slice(r.dataview.pagesize * r.dataview.pagenum, r.dataview.pagesize * r.dataview.pagenum + r.dataview.pagesize);
                                "root" == this.pageSizeMode && (w = h(d))
                            } else var w = d;
                            s = w
                        }
                    }
                    r.renderedRecords = s;
                    var p = s.length,
                        u = r.tableZIndex,
                        f = 0,
                        v = 0;
                    if (l) for (j = 0; j < a; j++) {
                        (S = (q = r.columns.records[j]).width) < q.minwidth && (S = q.minwidth), S > q.maxwidth && (S = q.maxwidth);
                        B = e('<table><tr><td role="gridcell" style="max-width: ' + S + "px; width:" + S + 'px;" class="' + C + '"></td></tr></table>');
                        e(document.body).append(B);
                        var x = B.find("td");
                        f = 1 + parseInt(x.css("padding-left")) + parseInt(x.css("padding-right")), B.remove();
                        break
                    }
                    for (var b = r.rtl ? " " + r.toTP("jqx-grid-table-rtl") : "", m = "<table cellspacing='0' class='" + r.toTP("jqx-grid-table") + b + "' id='table" + r.element.id + "'><colgroup>", y = "<table cellspacing='0' class='" + r.toTP("jqx-grid-table") + b + "' id='pinnedtable" + r.element.id + "'><colgroup>", _ = null, j = 0; j < a; j++) {
                        var q = r.columns.records[j];
                        if (!q.hidden) {
                            if (_ = q, (S = q.width) < q.minwidth && (S = q.minwidth), S > q.maxwidth && (S = q.maxwidth), (S -= f) < 0 && (S = 0), l) {
                                G = S;
                                0 == j && G++ , y += "<col style='max-width: " + S + "px; width: " + G + "px;'>", m += "<col style='max-width: " + S + "px; width: " + G + "px;'>"
                            } else y += "<col style='max-width: " + S + "px; width: " + S + "px;'>", m += "<col style='max-width: " + S + "px; width: " + S + "px;'>";
                            v += S
                        }
                    }
                    m += "</colgroup>", y += "</colgroup>", r._hiddencolumns = !1;
                    var R = !1;
                    if (0 === p) {
                        var k = '<tr role="row">',
                            T = r.host.height();
                        if (r.pageable && (T -= r.pagerHeight, "both" === r.pagerPosition && (T -= r.pagerHeight)), T -= r.columnsHeight, r.filterable) {
                            var P = r.filter.find(".filterrow"),
                                z = 1;
                            r.filter.find(".filterrow-hidden").length > 0 && (z = 0), T -= r.filterHeight + r.filterHeight * P.length * z
                        }
                        r.showstatusbar && (T -= r.statusBarHeight), r.showAggregates && (T -= r.aggregatesHeight), T < 25 && (T = 25), "hidden" != r.hScrollBar[0].style.visibility && (T -= r.hScrollBar.outerHeight()), ("auto" === r.height || null === r.height || r.autoheight) && (T = 100);
                        var S = r.host.width() + 2,
                            B = '<td colspan="' + r.columns.records.length + '" role="gridcell" style="border-right-color: transparent; min-height: ' + T + "px; height: " + T + "px;  min-width:" + v + "px; max-width:" + v + "px; width:" + v + "px;",
                            C = r.toTP("jqx-cell") + " " + r.toTP("jqx-grid-cell") + " " + r.toTP("jqx-item");
                        B += '" class="' + (C += " " + r.toTP("jqx-center-align")) + '">', r._loading || (B += r.gridlocalization.emptydatastring), m += k += B += "</td>", y += k, r.table[0].style.width = v + 2 + "px", o = v
                    }
                    for (var D = r.source._source.hierarchy && r.source._source.hierarchy.groupingDataFields ? r.source._source.hierarchy.groupingDataFields.length : 0, L = 0; L < s.length; L++) {
                        var M = s[L],
                            E = M.uid;
                        D > 0 && M[i.level] < D && (E = M.uid), void 0 === M.uid && (M.uid = r.dataview.generatekey());
                        var k = '<tr data-key="' + E + '" role="row" id="row' + L + r.element.id + '">',
                            F = '<tr data-key="' + E + '" role="row" id="row' + L + r.element.id + '">';
                        if (M.aggregate) var k = '<tr data-role="summaryrow" role="row" id="row' + L + r.element.id + '">',
                            F = '<tr data-role="summaryrow" role="row" id="row' + L + r.element.id + '">';
                        var H = 0;
                        if (r.rowinfo[E]) void 0 === r.rowinfo[E].checked && (r.rowinfo[E].checked = M[i.checked]), void 0 === r.rowinfo[E].icon && (r.rowinfo[E].icon = M[i.icon]), void 0 === r.rowinfo[E].aggregate && (r.rowinfo[E].aggregate = M[i.aggregate]), void 0 === r.rowinfo[E].row && (r.rowinfo[E].row = M), void 0 === r.rowinfo[E].leaf && (r.rowinfo[E].leaf = M[i.leaf]), void 0 === r.rowinfo[E].expanded && (r.rowinfo[E].expanded = M[i.expanded]);
                        else {
                            var A = M[i.checked];
                            void 0 === A && (A = !1), r.rowinfo[E] = {
                                selected: M[i.selected],
                                checked: A,
                                icon: M[i.icon],
                                aggregate: M.aggregate,
                                row: M,
                                leaf: M[i.leaf],
                                expanded: M[i.expanded]
                            }
                        }
                        var I = r.rowinfo[E];
                        I.row = M, M.originalRecord && (I.originalRecord = M.originalRecord);
                        for (var U = 0, j = 0; j < a; j++) {
                            var K = r.columns.records[j];
                            (K.pinned || r.rtl && r.columns.records[a - 1].pinned) && (R = !0), (S = K.width) < K.minwidth && (S = K.minwidth), S > K.maxwidth && (S = K.maxwidth), (S -= f) < 0 && (S = 0);
                            C = r.toTP("jqx-cell") + " " + r.toTP("jqx-grid-cell") + " " + r.toTP("jqx-item");
                            K.pinned && (C += " " + r.toTP("jqx-grid-cell-pinned")), r.sortcolumn === K.displayfield && (C += " " + r.toTP("jqx-grid-cell-sort")), r.altRows && L % 2 != 0 && (C += " " + r.toTP("jqx-grid-cell-alt")), r.rtl && (C += " " + r.toTP("jqx-cell-rtl"));
                            var V = "";
                            if (D > 0 && !l && !M.aggregate && M[i.level] < D) {
                                V += ' colspan="' + a + '"';
                                for (var G = 0, N = 0; N < a; N++) {
                                    var O = r.columns.records[N];
                                    if (!O.hidden) {
                                        var W = O.width;
                                        W < O.minwidth && (S = O.minwidth), W > O.maxwidth && (S = O.maxwidth), (W -= f) < 0 && (W = 0), G += W
                                    }
                                }
                                S = G
                            }
                            var B = '<td role="gridcell"' + V + ' style="max-width:' + S + "px; width:" + S + "px;",
                                J = '<td role="gridcell"' + V + ' style="pointer-events: none; visibility: hidden; border-color: transparent; max-width:' + S + "px; width:" + S + "px;";
                            j == a - 1 && 1 == a && (B += "border-right-color: transparent;", J += "border-right-color: transparent;"), D > 0 && M[i.level] < D && !M.aggregate ? r.rtl && (C += " " + r.toTP("jqx-right-align")) : "left" != K.cellsalign && ("right" === K.cellsalign ? C += " " + r.toTP("jqx-right-align") : C += " " + r.toTP("jqx-center-align")), I && (I.selected && r.editKey !== E && "none" !== r.selectionMode && (C += " " + r.toTP("jqx-grid-cell-selected"), C += " " + r.toTP("jqx-fill-state-pressed")), I.locked && (C += " " + r.toTP("jqx-grid-cell-locked")), I.aggregate && (C += " " + r.toTP("jqx-grid-cell-pinned"))), K.hidden ? (B += "display: none;", J += "display: none;", r._hiddencolumns = !0) : (0 != U || r.rtl ? (B += "border-right-width: 0px;", J += "border-right-width: 0px;") : (B += "border-left-width: 0px;", J += "border-left-width: 0px;"), U++ , H += f + S), K.pinned && (B += "pointer-events: auto;", J += "pointer-events: auto;");
                            var Z = "";
                            if (0 != r.source.hierarchy.length && M.records && (!M.records || 0 !== M.records.length) || this.virtualModeCreateRecords || (I.leaf = !0), M.records && M.records.length > 0 && (I.leaf = !1), r.dataview.filters.length > 0 && M.records && M.records.length > 0) {
                                for (var Q = !1, X = 0; X < M.records.length; X++) if (!1 !== M.records[X]._visible && void 0 == M.records[X].aggregate) {
                                    Q = !0;
                                    break
                                }
                                I.leaf = !Q
                            }
                            I && !I.leaf && (I.expanded ? (Z += r.toTP("jqx-tree-grid-expand-button") + " ", r.rtl ? Z += r.toTP("jqx-grid-group-expand-rtl") : Z += r.toTP("jqx-grid-group-expand"), Z += " " + r.toTP("jqx-icon-arrow-down")) : (Z += r.toTP("jqx-tree-grid-collapse-button") + " ", r.rtl ? (Z += r.toTP("jqx-grid-group-collapse-rtl"), Z += " " + r.toTP("jqx-icon-arrow-left")) : (Z += r.toTP("jqx-grid-group-collapse"), Z += " " + r.toTP("jqx-icon-arrow-right")))), (!r.autoRowHeight || 1 === U || r.autoRowHeight && !K.autoCellHeight) && (C += " " + r.toTP("jqx-grid-cell-nowrap"));
                            var Y = r._getcellvalue(K, I.row);
                            if (D > 0 && !M.aggregate && M[i.level] < D && (Y = M.label), "" != K.cellsFormat && e.jqx.dataFormat && (e.jqx.dataFormat.isDate(Y) ? Y = e.jqx.dataFormat.formatdate(Y, K.cellsFormat, r.gridlocalization) : (e.jqx.dataFormat.isNumber(Y) || !isNaN(parseFloat(Y)) && isFinite(Y)) && (Y = e.jqx.dataFormat.formatnumber(Y, K.cellsFormat, r.gridlocalization))), "" != K.cellclassname && K.cellclassname) if ("string" == typeof K.cellclassname) C += " " + K.cellclassname;
                            else {
                                var $ = K.cellclassname(L, K.datafield, r._getcellvalue(K, I.row), I.row, Y);
                                $ && (C += " " + $)
                            }
                            if ("" != K.cellsRenderer && K.cellsRenderer) {
                                var ee = K.cellsRenderer(E, K.datafield, r._getcellvalue(K, I.row), I.row, Y);
                                void 0 !== ee && (Y = ee)
                            }
                            if (I.aggregate && K.aggregates) {
                                var re = M.siblings.slice(0, M.siblings.length - 1),
                                    te = r._calculateaggregate(K, null, !0, re);
                                if (M[K.displayfield] = "", te) if (K.aggregatesRenderer) {
                                    if (te) {
                                        var ie = K.aggregatesRenderer(te[K.datafield], K, null, r.getcolumnaggregateddata(K.datafield, K.aggregates, !1, re), "subAggregates");
                                        Y = ie, M[K.displayfield] += name + ":" + te[K.datafield] + "\n"
                                    }
                                } else Y = "", M[K.displayfield] = "", e.each(te, function () {
                                    var e = this;
                                    for (obj in e) {
                                        var t = obj,
                                            i = '<div style="position: relative; margin: 0px; overflow: hidden;">' + (t = r._getaggregatename(t)) + ":" + e[obj] + "</div>";
                                        Y += i, M[K.displayfield] += t + ":" + e[obj] + "\n"
                                    }
                                });
                                else Y = ""
                            }
                            if (1 === U && !r.rtl || K == _ && r.rtl || D > 0 && M[i.level] < D) {
                                for (var oe = "", ae = r.toThemeProperty("jqx-tree-grid-indent"), le = I.leaf ? 1 : 0, se = 0; se < M[i.level] + le; se++) oe += "<span class='" + ae + "'></span>";
                                var ne = "<span class='" + Z + "'></span>",
                                    de = "",
                                    ce = "";
                                if (this.checkboxes && !M.aggregate) {
                                    var he = r.toThemeProperty("jqx-tree-grid-checkbox") + " " + ae + " " + r.toThemeProperty("jqx-checkbox-default") + " " + r.toThemeProperty("jqx-fill-state-normal") + " " + r.toThemeProperty("jqx-rc-all"),
                                        ge = !0;
                                    if (e.isFunction(this.checkboxes) && void 0 == (ge = this.checkboxes(E, M)) && (ge = !1), ge) if (I) {
                                        var we = I.checked;
                                        0 == this.hierarchicalCheckboxes && null === we && (we = !1), de += we ? "<span class='" + he + "'><div class='" + r.toThemeProperty("jqx-tree-grid-checkbox-tick") + " " + r.toThemeProperty("jqx-checkbox-check-checked") + "'></div></span>" : !1 === we ? "<span class='" + he + "'></span>" : "<span class='" + he + "'><div class='" + r.toThemeProperty("jqx-tree-grid-checkbox-tick") + " " + r.toThemeProperty("jqx-checkbox-check-indeterminate") + "'></div></span>"
                                    } else de += "<span class='" + he + "'></span>"
                                }
                                if (this.icons && !M.aggregate) {
                                    pe = r.toThemeProperty("jqx-tree-grid-icon") + " " + ae;
                                    if (r.rtl) var pe = r.toThemeProperty("jqx-tree-grid-icon") + " " + r.toThemeProperty("jqx-tree-grid-icon-rtl") + " " + ae;
                                    var ue = r.toThemeProperty("jqx-tree-grid-icon-size") + " " + ae,
                                        fe = I.icon;
                                    e.isFunction(this.icons) && (I.icon = this.icons(E, M), I.icon && (fe = !0)), fe && (I.icon ? ce += "<span class='" + pe + "'><img class='" + ue + "' src='" + I.icon + "'/></span>" : ce += "<span class='" + pe + "'></span>")
                                }
                                var ve = r.autoRowHeight && 1 === U && K.autoCellHeight ? " " + r.toTP("jqx-grid-cell-wrap") : "",
                                    xe = oe + ne + de + ce + "<span class='" + r.toThemeProperty("jqx-tree-grid-title") + ve + "'>" + Y + "</span>";
                                Y = r.rtl ? "<span class='" + r.toThemeProperty("jqx-tree-grid-title") + ve + "'>" + Y + "</span>" + ce + de + ne + oe : xe
                            }
                            if (D > 0 && l && j >= D && M[i.level] < D && (B += "padding-left: 5px; border-left-width: 0px;", J += "padding-left: 5px; border-left-width: 0px;", Y = "<span style='visibility: hidden;'>-</span>"), B += '" class="' + C + '">', B += Y, B += "</td>", J += '" class="' + C + '">', J += Y, J += "</td>", K.pinned ? (F += B, k += B) : (k += B, R && (F += J)), D > 0 && !l && M[i.level] < D && !M.aggregate) break
                        }
                        if (0 == o && (r.table[0].style.width = H + 2 + "px", o = H), k += "</tr>", F += "</tr>", m += k, y += F, r.rowDetails && !M.aggregate && this.rowDetailsRenderer) {
                            var be = '<tr data-role="row-details"><td valign="top" align="left" style="pointer-events: auto; max-width:' + S + "px; width:" + S + 'px; overflow: hidden; border-left: none; border-right: none;" colspan="' + r.columns.records.length + '" role="gridcell"',
                                C = r.toTP("jqx-cell") + " " + r.toTP("jqx-grid-cell") + " " + r.toTP("jqx-item");
                            C += " " + r.toTP("jqx-details"), C += " " + r.toTP("jqx-reset");
                            var me = this.rowDetailsRenderer(E, M);
                            me && (m += be += '" class="' + C + '"><div style="pointer-events: auto; overflow: hidden;"><div data-role="details">' + me + "</div></div></td></tr>", y += be)
                        }
                    }
                    if (m += "</table>", y += "</table>", R) {
                        r.WinJS ? MSApp.execUnsafeLocalFunction(function () {
                            r.table.html(y + m)
                        }) : r.table[0].innerHTML = y + m;
                        var ye = r.table.find("#table" + r.element.id),
                            _e = r.table.find("#pinnedtable" + r.element.id);
                        _e.css("float", "left"), _e.css("pointer-events", "none"), ye.css("float", "left"), _e[0].style.position = "absolute", ye[0].style.position = "relative", ye[0].style.zIndex = u - 10, _e[0].style.zIndex = u + 10, r._table = ye, r._table[0].style.left = "0px", r._pinnedTable = _e, l && (_e[0].style.left = "0px"), r._table[0].style.width = o + "px", r._pinnedTable[0].style.width = o + "px", r.rtl && r._haspinned && (r._pinnedTable[0].style.left = 3 - o + parseInt(r.element.style.width) + "px")
                    } else {
                        r.WinJS ? MSApp.execUnsafeLocalFunction(function () {
                            r.table.html(m)
                        }) : r.table[0].innerHTML = m;
                        N = r.table.find("#table" + r.element.id);
                        r._table = N, e.jqx.browser.msie && e.jqx.browser.version < 10 && (r._table[0].style.width = o + "px"), 0 === p && (r._table[0].style.width = 2 + o + "px")
                    }
                    0 === p && (r._table[0].style.tableLayout = "auto", r._pinnedTable && (r._pinnedTable[0].style.tableLayout = "auto")), r.showAggregates && r._updatecolumnsaggregates(), r._loading && 0 == p && (r._arrange(), this._showLoadElement()), r.rendered && r.rendered()
                }
            },
            propertyChangedHandler: function (r, t, i, o) {
                if (void 0 != r.isInitialized && 0 != r.isInitialized) {
                    var a = r.base;
                    if ("pageSizeMode" == t || "hierarchicalCheckboxes" == t) r._renderrows();
                    else if ("filterable" == t) a._render();
                    else if ("height" === t) a.host.height(r.height), a.host.width(r.width), a._updatesize(!1, !0);
                    else if ("width" === t) a.host.height(r.height), a.host.width(r.width), a._updatesize(!0, !1);
                    else if ("source" === t) a.updateBoundData();
                    else if ("columns" === t || "columnGroups" === t) a._columns = null, a._render();
                    else if ("rtl" === t) a.content.css("left", ""), r.columns = r._columns, a.vScrollBar.jqxScrollBar({
                        rtl: o
                    }), a.hScrollBar.jqxScrollBar({
                        rtl: o
                    }), a._render();
                    else if ("pagerMode" === t) r.pagerMode = o, a._initpager();
                    else if ("pageSizeOptions" == t) {
                        a._initpager();
                        for (var l = !1, s = 0; s < o.length; s++) if (parseInt(o[s]) == r.pageSize) {
                            l = !0;
                            break
                        }
                        l || e.jqx.set(r, [{
                            pageSize: o[0]
                        }])
                    } else if ("pageSize" == t) {
                        var n = a.dataview.pagenum * a.dataview.pagesize;
                        a.dataview.pagesize = a.pageSize;
                        var d = Math.floor(n / a.dataview.pagesize);
                        d === a.dataview.pagenum && parseInt(o) === parseInt(i) || (r._raiseEvent("pageSizeChanged", {
                            pagenum: o,
                            oldpageSize: i,
                            pageSize: a.dataview.pagesize
                        }), r.goToPage(d) || a.refresh())
                    } else if ("pagerPosition" === t) a.refresh();
                    else if ("selectionMode" === t) a.selectionMode = o.toLowerCase();
                    else if ("touchmode" == t) a.touchDevice = null, a._removeHandlers(), a.touchDevice = null, a.vScrollBar.jqxScrollBar({
                        touchMode: o
                    }), a.hScrollBar.jqxScrollBar({
                        touchMode: o
                    }), a._updateTouchScrolling(), a._arrange(), a._updatecolumnwidths(), a._renderrows(), a._addHandlers();
                    else {
                        if ("enableHover" == t) return;
                        if ("disabled" == t) o ? a.base.host.addClass(this.toThemeProperty("jqx-fill-state-disabled")) : a.base.host.removeClass(this.toThemeProperty("jqx-fill-state-disabled")), a.pageable && (a.pagernexttop && (a.pagernexttop.jqxButton({
                            disabled: o
                        }), a.pagerprevioustop.jqxButton({
                            disabled: o
                        }), a.pagernextbottom.jqxButton({
                            disabled: o
                        }), a.pagerpreviousbottom.jqxButton({
                            disabled: o
                        }), a.pagerfirsttop.jqxButton({
                            disabled: o
                        }), a.pagerfirstbottom.jqxButton({
                            disabled: o
                        }), a.pagerlasttop.jqxButton({
                            disabled: o
                        }), a.pagerlastbottom.jqxButton({
                            disabled: o
                        }), a.pagershowrowscombotop.jqxDropDownList && "advanced" == a.pagerMode && (a.pagershowrowscombotop.jqxDropDownList({
                            disabled: !1
                        }), a.pagershowrowscombobottom.jqxDropDownList({
                            disabled: !1
                        }))), a.base.host.find(".jqx-grid-pager-number").css("cursor", o ? "default" : "pointer")), a.base.host.find(".jqx-grid-group-collapse").css("cursor", o ? "default" : "pointer"), a.base.host.find(".jqx-grid-group-expand").css("cursor", o ? "default" : "pointer");
                        else if ("columnsHeight" == t) a._render();
                        else if ("theme" == t) {
                            if (e.jqx.utilities.setTheme(i, o, a.base.host), a.vScrollBar.jqxScrollBar({
                                theme: a.theme
                            }), a.hScrollBar.jqxScrollBar({
                                theme: a.theme
                            }), a.pageable && a.pagernexttop && (a.pagernexttop.jqxButton({
                                theme: a.theme
                            }), a.pagerprevioustop.jqxButton({
                                theme: a.theme
                            }), a.pagernextbottom.jqxButton({
                                theme: a.theme
                            }), a.pagerpreviousbottom.jqxButton({
                                theme: a.theme
                            }), a.pagerfirsttop.jqxButton({
                                theme: a.theme
                            }), a.pagerfirstbottom.jqxButton({
                                theme: a.theme
                            }), a.pagerlasttop.jqxButton({
                                theme: a.theme
                            }), a.pagerlastbottom.jqxButton({
                                theme: a.theme
                            }), a.pagershowrowscombotop.jqxDropDownList && "advanced" == a.pagerMode && (a.pagershowrowscombotop.jqxDropDownList({
                                theme: a.theme
                            }), a.pagershowrowscombobottom.jqxDropDownList({
                                theme: a.theme
                            }))), a.filterable) {
                                var c = e(".filterconditions");
                                c.length > 0 && c.jqxDropDownList({
                                    theme: a.theme
                                }), a.filtercolumnsList && a.filtercolumnsList.jqxDropDownList({
                                    theme: a.theme
                                })
                            }
                            a.refresh()
                        } else a.refresh()
                    }
                }
            },
            checkRow: function (e, r, t) {
                var i = this.base,
                    o = i._names();
                if (!i._loading) {
                    var a = i.rowinfo[e];
                    if (a) a.checked = !0, a.row[o.checked] = !0, a.originalRecord && (a.originalRecord[o.checked] = !0), void 0 == t && this.hierarchicalCheckboxes && this.checkRows(a.row, a.row), !1 !== r && i._renderrows(), i._raiseEvent("rowCheck", {
                        key: e,
                        row: a.row
                    });
                    else {
                        var l = this.getRow(e);
                        l && (i.rowinfo[e] = {
                            row: l,
                            checked: !0
                        }, i.rowinfo[e].row[o.checked] = !0, l.originalRecord && (i.rowinfo[e].originalRecord = l.originalRecord), i._raiseEvent("rowCheck", {
                            key: e,
                            row: l
                        }), void 0 == t && this.hierarchicalCheckboxes && this.checkRows(l, l), !1 !== r && i._renderrows())
                    }
                }
            },
            checkRows: function (r, t) {
                var i = this,
                    o = this.base._names(),
                    a = function (e) {
                        var r = new Array,
                            t = function (e) {
                                for (var i = 0; i < e.length; i++) r.push(e[i]), e[i] && e[i].records && t(e[i].records)
                            };
                        return e.records && t(e.records), r
                    };
                if (null != r) {
                    var l = 0,
                        s = !1,
                        n = 0,
                        d = function (e) {
                            for (var r = 0; r < e.length; r++) if (e[r]) {
                                var t = e[r][o.checked];
                                void 0 === t && (t = !1), 0 != t && (null == e[r][o.checked] && (s = !0), e[r].records && d(e[r].records), l++), n++
                            }
                        };
                    if (r.records && d(r.records), r != t) l == n ? this.checkRow(r.uid, !1, "tree") : l > 0 ? this.indeterminateRow(r.uid, !1, "tree") : this.uncheckRow(r.uid, !1, "tree");
                    else {
                        var c = t[o.checked],
                            h = a(t);
                        e.each(h, function () {
                            !0 === c ? i.checkRow(this.uid, !1, "tree") : !1 === c ? i.uncheckRow(this.uid, !1, "tree") : i.indeterminateRow(this.uid, !1, "tree")
                        })
                    }
                    var g = r[o.parent] ? r[o.parent] : null;
                    this.checkRows(g, t)
                } else {
                    var c = t[o.checked],
                        h = a(t);
                    e.each(h, function () {
                        !0 === c ? i.checkRow(this.uid, !1, "tree") : !1 === c ? i.uncheckRow(this.uid, !1, "tree") : i.indeterminateRow(this.uid, !1, "tree")
                    })
                }
            },
            indeterminateRow: function (e, r, t) {
                var i = this.base,
                    o = i._names();
                if (!i._loading) {
                    var a = i.rowinfo[e];
                    if (a) a.checked = null, a.row[o.checked] = null, a.originalRecord && (a.originalRecord[o.checked] = null), void 0 == t && this.hierarchicalCheckboxes && this.checkRows(a.row, a.row), !1 !== r && i._renderrows();
                    else {
                        var l = this.getRow(e);
                        l && (i.rowinfo[e] = {
                            row: l,
                            checked: null
                        }, i.rowinfo[e].row[o.checked] = null, l.originalRecord && (i.rowinfo[e].originalRecord = l.originalRecord), void 0 == t && this.hierarchicalCheckboxes && this.checkRows(l, l), !1 !== r && i._renderrows())
                    }
                }
            },
            uncheckRow: function (e, r, t) {
                var i = this.base,
                    o = i._names();
                if (!i._loading) {
                    var a = i.rowinfo[e];
                    if (a) a.checked = !1, a.row[o.checked] = !1, a.originalRecord && (a.originalRecord[o.checked] = !1), void 0 == t && this.hierarchicalCheckboxes && this.checkRows(a.row, a.row), !1 !== r && i._renderrows(), i._raiseEvent("rowUncheck", {
                        key: e,
                        row: a.row
                    });
                    else {
                        var l = this.getRow(e);
                        l && (i.rowinfo[e] = {
                            row: l,
                            checked: !1
                        }, i.rowinfo[e].row[o.checked] = !1, l.originalRecord && (i.rowinfo[e].originalRecord = l.originalRecord), i._raiseEvent("rowUncheck", {
                            key: e,
                            row: l
                        }), void 0 == t && this.hierarchicalCheckboxes && this.checkRows(l, l), !1 !== r && i._renderrows())
                    }
                }
            },
            expandRows: function (r) {
                var t = this;
                if (r) if (t.virtualModeCreateRecords) e.each(r, function () {
                    var e = this;
                    t.base._loading = !1, t.expandRow(e.uid, function () {
                        t.base._loading = !1, t.expandRows(e.records)
                    })
                });
                else for (var i = 0; i < r.length; i++) {
                    var o = r[i];
                    t.expandRow(o.uid), t.expandRows(o.records)
                }
            },
            collapseRows: function (e) {
                if (e) for (var r = 0; r < e.length; r++) this.collapseRow(e[r].uid), this.collapseRows(e[r].records)
            },
            expandAll: function () {
                var e = this.base;
                e.beginUpdate(), this.expandRows(this.getRows()), e.endUpdate()
            },
            collapseAll: function () {
                var e = this.base;
                e.beginUpdate(), this.collapseRows(this.getRows()), e.endUpdate()
            },
            expandRow: function (e, r) {
                var t = this.base;
                if (!t._loading) {
                    var i = t._names(),
                        o = this,
                        a = t.rowinfo[e];
                    if (!a) {
                        var l = this.getRow(e);
                        l && (t.rowinfo[e] = {
                            row: l
                        }, l.originalRecord && (t.rowinfo[e].originalRecord = l.originalRecord), a = t.rowinfo[e])
                    }
                    if (a) {
                        if (a.expanded) return void (a.row[i.expanded] = !0);
                        if (a.expanded = !0, a.row[i.expanded] = !0, a.originalRecord && (a.originalRecord[i.expanded] = !0), this.virtualModeCreateRecords && !a.row._loadedOnDemand) {
                            if (!a.row[i.leaf]) return t._loading = !0, this._showLoadElement(), void this.virtualModeCreateRecords(a.row, function (e) {
                                if (a.row._loadedOnDemand = !0, !1 === e) return t._loading = !1, o._hideLoadElement(), a.leaf = !0, a.row[i.leaf] = !0, t._renderrows(), void (r && r());
                                for (var l = 0; l < e.length; l++) {
                                    if (e[l][i.level] = a.row[i.level] + 1, e[l][i.parent] = a.row, t.rowsByKey[e[l].uid]) throw t._loading = !1, o._hideLoadElement(), a.leaf = !0, a.row[i.leaf] = !0, t._renderrows(), r && r(), new Error("Please, check whether you Add Records with unique ID/Key. ");
                                    t.rowsByKey[e[l].uid] = e[l], o.virtualModeRecordCreating(e[l])
                                }
                                a.row.records ? a.row.records = a.row.records.concat(e) : a.row.records = e, (!e || e && 0 == e.length) && (a.leaf = !0, a.row[i.leaf] = !0), a.originalRecord && (a.originalRecord.records = e, a.originalRecord[i.expanded] = !0, 0 == e.length && (a.originalRecord[i.leaf] = !0)), t._loading = !1, o._hideLoadElement();
                                var s = t.vScrollBar.css("visibility");
                                t._renderrows(), t._updateScrollbars();
                                var n = s != t.vScrollBar.css("visibility");
                                ("auto" === t.height || null === t.height || t.autoheight || n) && t._arrange(), t._renderhorizontalscroll(), r && r()
                            })
                        }
                        if (!t.updating()) {
                            var s = t.vScrollBar.css("visibility");
                            t._renderrows(), t._updateScrollbars();
                            var n = s != t.vScrollBar.css("visibility");
                            ("auto" === t.height || null === t.height || t.autoheight || n) && t._arrange(), t._renderhorizontalscroll(), t._raiseEvent("rowExpand", {
                                row: a.row,
                                key: e
                            })
                        }
                    }
                }
            },
            collapseRow: function (e) {
                var r = this.base,
                    t = r._names();
                if (!r._loading) {
                    var i = r.rowinfo[e];
                    if (!i) {
                        var o = this.getRow(e);
                        o && (r.rowinfo[e] = {
                            row: o
                        }, o.originalRecord && (r.rowinfo[e].originalRecord = o.originalRecord), i = r.rowinfo[e])
                    }
                    if (i) {
                        if (!i.expanded) return void (i.row[t.expanded] = !1);
                        if (i.expanded = !1, i.row[t.expanded] = !1, i.originalRecord && (i.originalRecord[t.expanded] = !1), !r.updating()) {
                            var a = r.vScrollBar.css("visibility");
                            r._renderrows(), r._updateScrollbars();
                            var l = a != r.vScrollBar.css("visibility");
                            ("auto" === r.height || null === r.height || r.autoheight || l) && r._arrange(), r._renderhorizontalscroll(), r._raiseEvent("rowCollapse", {
                                row: i.row,
                                key: e
                            })
                        }
                    }
                }
            }
        })
}(jqxBaseFramework);