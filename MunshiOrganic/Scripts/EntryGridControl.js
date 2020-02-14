
(function($){
//    var $mainTable;
//    var $inputRowTable;
    var inputRowTableWidth;
    var $containerDiv;
    var inputColumns;
    var $document = $(document);
    var mainTableId;
    var inputRowTableId;
    var curRowNo = 1;
    var mainTableLength = 1;
    var tableHeaderStyle = null;
    var lastControlBeforeTable = null;
    var firstControlAfterTable = null;
    var navigateOutOfGridWhenColumnNoEmpty = null;
    var addColumnForDeleteButton = false;
    var jsonData = null;
    var allowAddNewRow = true;
//    var primaryKeyJsonFld='';
//    var databaseKeyJsonFld=null;
    var resourcesPath = '';
    var newRowPrimaryKeySeedValue = 8888990;
    var deletedRowPrimaryKeySeedValue = 9999990;

    //Constants
    var SELECT2_ID_PREFIX = 's2id';
    var DELETE_COLUMN_WIDTH = '40px';
    //constants for column Classes
    var CLASS_BUTTON_COLUMN = 'buttonColumn';
    var CLASS_HIDDEN_COLUMN = 'hiddenColumn';
    var CLASS_BUTTONS_COLUMN = 'buttonsColumn';
    //constants for Div, InputTable, Control Classes
    var CLASS_TABLE_CONTAINER = 'tableContainer';
    var CLASS_INPUT_ROW_TABLE = 'inputRowTable';
    var CLASS_INPUT_ROW_CONTROL = 'inputRowControl';
    var CLASS_INPUT_ROW_BUTTON = 'inputRowButton';
    var CLASS_INPUT_ROW_HIDDEN = 'inputRowHidden';
    var CLASS_DELETE_ROW_BUTTON = 'deleteRowButton';
    var CLASS_TABLE_ENTRY_GRID = 'tableEntryGrid';
    var CLASS_BEFORE_TABLE_ENTRY_GRID = 'beforeTableEntryGrid';
    var CLASS_AFTER_TABLE_ENTRY_GRID = 'afterTableEntryGrid';
    var ATTR_BEFORE_CONTAINER_DIV_ID = 'beforeContainerDivId';
    var ATTR_AFTER_CONTAINER_DIV_ID = 'afterContainerDivId';
    var CLASS_READ_ONLY_LABEL = 'readOnlyLabel';
    //constants for Json Fields
    var TEXT_JSON_FLD = 'textJsonFld';
    var VAL_JSON_FLD = 'valJsonFld';
    //Action Constants
    var EMPTY_JSON = -1;
    var NO_ACTION = 0;
    var ROW_ADDED = 1;
    var ROW_EDITED = 2;
    var ROW_DELETED = 3;
    //MoveRow Constants
    var DONT_MOVE = 0;
    var NEXT_ROW = 1;
    var PREV_ROW = -1;

    var KEY = {
        TAB: 9,
        ENTER: 13,
        ESC: 27,
        SPACE: 32,
        LEFT: 37,
        UP: 38,
        RIGHT: 39,
        DOWN: 40,
        SHIFT: 16,
        CTRL: 17,
        ALT: 18,
        PAGE_UP: 33,
        PAGE_DOWN: 34,
        HOME: 36,
        END: 35,
        BACKSPACE: 8,
        DELETE: 46,
        isArrow: function (k) {
            k = k.which ? k.which : k;
            switch (k) {
                case KEY.LEFT:
                case KEY.RIGHT:
                case KEY.UP:
                case KEY.DOWN:
                    return true;
            }
            return false;
        },
        isControl: function (e) {
            var k = e.which;
            switch (k) {
                case KEY.SHIFT:
                case KEY.CTRL:
                case KEY.ALT:
                    return true;
            }

            if (e.metaKey) return true;

            return false;
        },
        isFunctionKey: function (k) {
            k = k.which ? k.which : k;
            return k >= 112 && k <= 123;
        }
    }

    $.widget("natural.EntryGrid", {

        options: {
            lastControlBeforeTable: null,
            firstControlAfterTable: null,
            tableHeaderStyle: null,
            addColumnForDeleteButton: false,
            navigateOutOfGridWhenColumnNoEmpty: 0,
            jsonData: null,
            primaryKeyJsonFld:'',
            databaseKeyJsonFld:null,
            resourcesPath:'',
            rowChange:null,
            emptyNewJsonRowData: true,
            allowAddNewRow: true
        },

        _create: function () {

            this._setOptions({
                lastControlBeforeTable: this.options.lastControlBeforeTable,
                firstControlAfterTable: this.options.firstControlAfterTable,
                tableHeaderStyle: this.options.tableHeaderStyle,
                addColumnForDeleteButton: this.options.addColumnForDeleteButton,
                navigateOutOfGridWhenColumnNoEmpty: this.options.navigateOutOfGridWhenColumnNoEmpty,
                jsonData: this.options.jsonData,
                primaryKeyJsonFld: this.options.primaryKeyJsonFld,
                databaseKeyJsonFld: this.options.databaseKeyJsonFld,
                resourcesPath: this.option.resourcesPath,
                rowChange: this.option.rowChange,
                emptyNewJsonRowData: this.options.emptyNewJsonRowData,
                allowAddNewRow: this.options.allowAddNewRow
            });

//            console.log('_create: options', this.options);

            var $mainTable = $("<table class='tableEntryGrid' style='height: 100%; border-left: 1px solid #C0D0E5; table-layout: fixed;' cellspacing='0' >");

            curRowNo = 1;
            mainTableLength = 1;
            $containerDiv = this.element;

            if (!$containerDiv.hasClass(CLASS_TABLE_CONTAINER)){
                $containerDiv.addClass(CLASS_TABLE_CONTAINER);
            }

            if (!validateHTMLAndJSONData($containerDiv, this.options)){
                console.error('Invalid HTML specification. Please check Entry Grid HTML & JSON');
                return null;
            }

            var $inputRowTable = $containerDiv.find(".inputRowTable" );
            mainTableId = 'EntryGrid_' + $containerDiv.attr('id');
            $mainTable.attr('id', mainTableId);
            $mainTable.attr('containerDiv', $containerDiv.attr('id'));

            inputRowTableWidth = $inputRowTable.css('width');



            $mainTable.attr(ATTR_BEFORE_CONTAINER_DIV_ID, $('#' + lastControlBeforeTable).attr('id'));
            $mainTable.attr(ATTR_AFTER_CONTAINER_DIV_ID, $('#' + firstControlAfterTable).attr('id'));
            $('#' + lastControlBeforeTable).addClass(CLASS_BEFORE_TABLE_ENTRY_GRID);
            $('#' + lastControlBeforeTable).attr(ATTR_BEFORE_CONTAINER_DIV_ID, $containerDiv.attr('id'));

            $('#' + firstControlAfterTable).addClass(CLASS_AFTER_TABLE_ENTRY_GRID);
            $('#' + firstControlAfterTable).attr(ATTR_AFTER_CONTAINER_DIV_ID, $containerDiv.attr('id'));

            if (addColumnForDeleteButton){
                $mainTable.css('width', $inputRowTable.css('width'));
            }else {
                $mainTable.css('width', $inputRowTable.css('width'));
            }
//            console.log('main table width', $mainTable.css('width'));

            var valJsonFld = 'AcntId';

            $mainTable.css('left', $inputRowTable.css('left'));
            $inputRowTable.attr('curRowNo', 0);
            $inputRowTable.hide();

            $mainTable.on("click", "td", function(event){
//                console.log("$mainTable.click.td: index", $(this).attr('id'));

//                console.log('Maintable.containerDiv: ', $(this).closest('.' + CLASS_TABLE_ENTRY_GRID).attr('containerDiv'));
//                console.log('Maintable.inputRowTable: ', $('#' + $(this).closest('.' + CLASS_TABLE_ENTRY_GRID).attr('containerDiv')).find('.' + CLASS_INPUT_ROW_TABLE).attr('id'));

                var currentMainTable = $(this).closest('.' + CLASS_TABLE_ENTRY_GRID);
                var currentContainerDiv = $('#' + $(currentMainTable).attr('containerDiv'));
                var currentInputRowTable = currentContainerDiv.find('.' + CLASS_INPUT_ROW_TABLE);
                mainTableLength = currentMainTable.find('tr').length;

//                console.log('Maintable.2MainTable: ', currentMainTable.attr('id'));
//                console.log('Maintable.2containerDiv: ', currentContainerDiv.attr('id'));
//                console.log('Maintable.2inputRowTable: ', currentInputRowTable.attr('id'));


                var isNewRow = false;
                var $tr = $(this).closest('tr');

                //        console.log("$mainTableClick: row no", $tr.index());
                //        console.log("column text", $(this).siblings("td:first").text());
                curRowNo = $tr.index();

                if ($(this).attr('class') == CLASS_DELETE_ROW_BUTTON){
                    //            console.log('$mainTable.Click td: delete Row');
                    //deleteRowFromMainTable($tr);
                    return false;
                }

                if ($(this).attr('class') == 'buttonsColumn'){
                    return false;
                }

                if (curRowNo == mainTableLength - 1){
                    addRowToMainTable(null, currentMainTable, currentInputRowTable);
                    isNewRow = true;
                }
                showControls($tr.index(), $(this).index(), isNewRow, currentContainerDiv, currentMainTable, currentInputRowTable);
                setFocusOnColumnControl($(this), currentInputRowTable);

            });

            $mainTable.on("click", "button", function (event) {
                var currentMainTable = $(this).closest('.' + CLASS_TABLE_ENTRY_GRID);
                var currentContainerDiv = $('#' + $(this).closest('.' + CLASS_TABLE_ENTRY_GRID).attr('containerDiv'));
                var currentInputRowTable = currentContainerDiv.find('.' + CLASS_INPUT_ROW_TABLE);

                var $tr = $(this).closest('tr');
                curRowNo = $tr.index();

                if ($(this).hasClass(CLASS_DELETE_ROW_BUTTON)){
                    deleteRowFromMainTable($tr, currentContainerDiv, currentMainTable, currentInputRowTable);

                    return false;
                }
            });



            createTableHeader($mainTable, $inputRowTable, this.options.addColumnForDeleteButton);

            console.log('_create: jsonData', jsonData);

            if (jsonData != null){
                $.each(jsonData, function(index, obj){
                    if (obj.ActionFlag != ROW_DELETED && obj.ActionFlag != EMPTY_JSON)
                        addRowToMainTable(obj, $mainTable, $inputRowTable);
                });
                if (allowAddNewRow) {
                    if (jsonData.length == 1 && jsonData[0].ActionFlag == EMPTY_JSON) {
                        addRowToMainTable(null, $mainTable, $inputRowTable);
                    }
                    addRowToMainTable(null, $mainTable, $inputRowTable);
                }
            } else {
                if (allowAddNewRow) {
                    addRowToMainTable(null, $mainTable, $inputRowTable);
                    addRowToMainTable(null, $mainTable, $inputRowTable);
                }
            }


            $(this.element).append($mainTable);


        },

        _setOption: function (key, value) {
            switch (key) {
                case "lastControlBeforeTable":
                    lastControlBeforeTable = this.options.lastControlBeforeTable;
                    break;
                case "firstControlAfterTable":
                    firstControlAfterTable = this.options.firstControlAfterTable;
                    break;
                case "addColumnForDeleteButton":
                    addColumnForDeleteButton = this.options.addColumnForDeleteButton;
                    break;
                case "navigateOutOfGridWhenColumnNoEmpty":
                    navigateOutOfGridWhenColumnNoEmpty = this.options.navigateOutOfGridWhenColumnNoEmpty;
                    break;
                case "tableHeaderStyle":
                    tableHeaderStyle = this.options.tableHeaderStyle;
                    break;
                case "jsonData":
                    jsonData = this.options.jsonData;
                    break;
                case "allowAddNewRow":
                    allowAddNewRow = this.options.allowAddNewRow;
                case "primaryKeyJsonFld":
                    //primaryKeyJsonFld = this.options.primaryKeyJsonFld;
                    break;
                case "databaseKeyJsonFld":
                    //databaseKeyJsonFld = this.options.databaseKeyJsonFld;
                    break;
                case "resourcesPath":
                    resourcesPath = this.options.resourcesPath;
                    break;

            }
        },

        _setOptions: function( options ) {
            this._super( options );
        },

        getJsonData: function(deleteEmptyRows, returnPrimaryKeyAsGuid){
            var $containerDiv = this.element;
            var $mainTable = $containerDiv.find('.' + CLASS_TABLE_ENTRY_GRID);

            //var options = $containerDiv.EntryGrid('instance').options;
            var options = this.options;
            var curJsonData = options.jsonData;

            var primaryKeyJsonFld = options.primaryKeyJsonFld;
            var databaseKeyJsonFld = options.databaseKeyJsonFld;

            var returnJson = $.extend( {}, curJsonData );

                $.each(returnJson, function(index, obj){
                    //console.log('getJsonData: ActionFlg', obj.ActionFlag );
                    if (obj.ActionFlag == ROW_ADDED && returnPrimaryKeyAsGuid) {
                        //obj[primaryKeyJsonFld] = convertToGUID(obj[primaryKeyJsonFld]);
                    }
                    if (deleteEmptyRows) {
//                    $.each(obj, function(index, jsonElement){
//                        console.log('getJsonData', 'jsonElement:' + jsonElement);
//                    })
                    }
                });

            return curJsonData;
        },
        getJsonDataForRow: function(returnPrimaryKeyAsGuid, rowNo){
            var $containerDiv = this.element;
            var $mainTable = $containerDiv.find('.' + CLASS_TABLE_ENTRY_GRID);
            var $inputRowTable = $containerDiv.find('.' + CLASS_INPUT_ROW_TABLE);

            var options = $containerDiv.EntryGrid('instance').options;
            var curJsonData = options.jsonData;

            var primaryKeyJsonFld = $containerDiv.EntryGrid('instance').options.primaryKeyJsonFld;

            var colId = $inputRowTable.find('[valJsonFld = "' + primaryKeyJsonFld + '"]').attr('id');

            var rowIdValue = $('#label_' + $containerDiv.attr('id') + '_' + colId + '_' + rowNo).html() ;
            console.log('getJsonDataForRow: colId', colId);
            console.log('getJsonDataForRow: colId', colId);
            console.log('getJsonDataForRow: rowIdValue', rowIdValue);

            var returnJson = {};

            $.each(curJsonData, function(index, obj){
                //console.log('getJsonData: ActionFlg', obj.ActionFlag );
                if (obj[primaryKeyJsonFld] == rowIdValue) {
                    returnJson = obj;


                    //if (returnJson.ActionFlag == ROW_ADDED && returnPrimaryKeyAsGuid) {
                    //    returnJson[primaryKeyJsonFld] = convertToGUID(returnJson[primaryKeyJsonFld]);
                    //}
                }

            });

            return returnJson;
        },
        destroy: function() {
            $(this.element).find('.' + CLASS_TABLE_ENTRY_GRID).remove();
            // Call the base destroy function.
            $.Widget.prototype.destroy.call(this);
        },

        move: function (dx) {
            console.log('move: dx', dx);
//            var x = dx + parseInt(this._button.css("left"));
//            this._button.css("left", x);
        }
    });


    $document.click(function (e) {
       try {
            (document.activeElement).select();

       } catch (e) { }
    });

    $document.mouseup(function (e) {

        var focused = document.activeElement;
        console.log('document.mouseup: Focused Element id', $(focused).attr("id"));
        console.log('document.mouseup: Focused Element', $(focused));
        console.log('document.mouseup: Focused Element class', $(focused).attr('class'));
//        console.log('document.mouseup: Nearest TableEntryGrid id', $(focused).closest('.' + CLASS_TABLE_ENTRY_GRID).attr("id"));
        var attrId = $(focused).attr('id');
        var attrClass = $(focused).attr('class');
        if ((typeof attrId != typeof undefined && attrId != false) || (typeof attrClass != typeof undefined && attrClass != false)) {
            if (!$(focused).hasClass("tableEntryGrid")) {
                if (!$(focused).hasClass("inputRowTable")) {
                    if (!$(focused).hasClass("inputRowControl")) {
                        if (!$(focused).closest('table').hasClass("inputRowTable") && !$(focused).hasClass("select2-container") && !$(focused).hasClass("select2-selection") && !$(focused).hasClass("select2-search__field") && !$(focused).hasClass("select2-input")) {
                            saveAndHideInputRowTables();
                        }
                    }
                }
            }
        }else{
            saveAndHideInputRowTables();
        }
    });

    function saveAndHideInputRowTables(){
        var visibleInputRowTables = $('.' + CLASS_INPUT_ROW_TABLE + ':visible');
        if (visibleInputRowTables.length > 0){
            visibleInputRowTables.each(function(index, obj){
//                console.log('index', index);
//                console.log('obj', obj);
                var containerDiv = $(obj).closest('.' + CLASS_TABLE_CONTAINER);
                curRowNo = parseInt($(obj).attr('curRowNo'),10);
                console.log('saveAndHideInputRowTables: curRowNo ', curRowNo);
                moveRow(DONT_MOVE, containerDiv);
            });
        }
        $('.'+CLASS_INPUT_ROW_TABLE).css('display','none');
    }

    $document.keydown(function(evt){
        var focused = document.activeElement;
//        console.log('document.keydown: focused id', $(focused).attr('id'));
//        console.log('document.keydown: containerDiv id', $(focused).closest('.' + CLASS_TABLE_CONTAINER).attr('id'));
//        console.log('document.keydown: first input control', $($(focused).closest('.' + CLASS_TABLE_CONTAINER)).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:first").attr("id"));
//        console.log('document.keydown: last input control', $($(focused).closest('.' + CLASS_TABLE_CONTAINER)).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:last").attr("id"));
        var currentContainerDiv = null;
        var currentMainTable = null;
        currentContainerDiv = $(focused).closest('.' + CLASS_TABLE_CONTAINER);
//        console.log('document.keyDown: currentContainerDiv', currentContainerDiv);
        if (currentContainerDiv.length != 0){
            currentMainTable = currentContainerDiv.find('.' + CLASS_TABLE_ENTRY_GRID);
            mainTableLength = currentMainTable.find('tr').length;
            if (currentMainTable != null) {
                lastControlBeforeTable = $('#' + currentMainTable.attr(ATTR_BEFORE_CONTAINER_DIV_ID));
                firstControlAfterTable = $('#' + currentMainTable.attr(ATTR_AFTER_CONTAINER_DIV_ID));
            }
        }else{
            currentContainerDiv = null;
        }
        if (evt.keyCode == KEY.TAB) {
            if (evt.shiftKey) {
                if (curRowNo == 1) {
                    if ($(focused).closest('td:visible').index() == 0) {
                        if (lastControlBeforeTable != null) {
                            moveRow(DONT_MOVE, currentContainerDiv);
                            lastControlBeforeTable.focus();
                            return false;
                        } else
                            console.log("Document.keyDown: lastControlBeforeTable is null");
                    }
                }
                if ($(focused).attr('id') == $(currentContainerDiv).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:first").attr("id")) {
                    curRowNo = parseInt($(focused).closest('.' + CLASS_INPUT_ROW_TABLE).attr('curRowNo'),10);
                    moveRow(PREV_ROW, currentContainerDiv);
                    $(currentContainerDiv).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:last").focus();
                    return false;
                }

                if ($(focused).hasClass(CLASS_AFTER_TABLE_ENTRY_GRID)) {
                    var currentContainerDivId = $(focused).attr(ATTR_AFTER_CONTAINER_DIV_ID);
                    currentContainerDiv = $('#' + currentContainerDivId);
                    currentMainTable = currentContainerDiv.find('.' + CLASS_TABLE_ENTRY_GRID);
                    mainTableLength = currentMainTable.find('tr').length;
                    curRowNo = mainTableLength - 1;

                    moveRow(PREV_ROW, currentContainerDiv);
                    $(currentContainerDiv).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:last").focus();
                    return false;
                }
            } else {
//                console.log('Document KeyDown: Tab key last Column Id', $( "." + CLASS_INPUT_ROW_CONTROL + ":enabled:last" ).attr("id"));
                if (curRowNo == mainTableLength - 2) {
                    if ($(focused).closest('td').index() == navigateOutOfGridWhenColumnNoEmpty) {
                        var controlValue = getControlValueForColumn($(focused).closest('td'));
                        if (controlValue != null) {
                            if (controlValue.text == '') {
                                if (firstControlAfterTable != null) {
                                    moveRow(DONT_MOVE, currentContainerDiv);
                                    firstControlAfterTable.focus();
                                    return false;
                                }
                                else
                                    console.log("Document.keyDown: firstControlAfterTable is null");
                            }
                        }
                    }
                }
                if ($(focused).attr('id') == $(currentContainerDiv).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:last").attr("id")) {
                    curRowNo = parseInt($(focused).closest('.' + CLASS_INPUT_ROW_TABLE).attr('curRowNo'),10);
                    console.log('document.keyDown curRowNo', curRowNo);
                    moveRow(NEXT_ROW, currentContainerDiv);

                    var controls = $(currentContainerDiv).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:first");
                    if (controls.length > 0) {
                        control = controls.first();
                        var controlType = control.get(0).tagName;
                        if (controlType == 'SELECT') {
                            for (var i = 0; i < control.get(0).classList.length; i++) {
                                if (control.get(0).classList[i].toLowerCase() == 'select2search') {
                                    controlType = 'SELECT2SEARCH';
                                    break;
                                }
                            }
                        }
                    }
                    $(currentContainerDiv).parent().scrollLeft(0);
                    if (controlType == 'SELECT2SEARCH') {
                        $('#' + $(currentContainerDiv).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:first").attr('id')).select2('open');
                    } else {
                        $(currentContainerDiv).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:first").focus();
                    }
                    return false;
                }

                if ($(focused).hasClass(CLASS_BEFORE_TABLE_ENTRY_GRID)) {
                    var currentContainerDivId = $(focused).attr(ATTR_BEFORE_CONTAINER_DIV_ID);
                    currentContainerDiv = $('#' + currentContainerDivId);
                    currentMainTable = currentContainerDiv.find('.' + CLASS_TABLE_ENTRY_GRID);
                    mainTableLength = currentMainTable.find('tr').length;
                    curRowNo = 0;
                    moveRow(NEXT_ROW, currentContainerDiv);
                    
                    var controls = $(currentContainerDiv).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:first");
                    if (controls.length > 0) {
                        control = controls.first();
                        var controlType = control.get(0).tagName;
                        if (controlType == 'SELECT') {
                            for (var i = 0; i < control.get(0).classList.length; i++) {
                                if (control.get(0).classList[i].toLowerCase() == 'select2search') {
                                    controlType = 'SELECT2SEARCH';
                                    break;
                                }
                            }
                        }
                    }
                    if (controlType == 'SELECT2SEARCH') {
                        $('#' + $(currentContainerDiv).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:first").attr('id')).select2('open');
                    } else {
                        $(currentContainerDiv).find("." + CLASS_INPUT_ROW_CONTROL + ":enabled:first").focus();
                    }
                    return false;
                }
            }
        } else if (evt.keyCode == KEY.UP) {
            if ($(focused).closest('table').attr('class') == CLASS_INPUT_ROW_TABLE) {
                if ($(focused).get(0).tagName != 'SELECT' || evt.ctrlKey) {
                    if (curRowNo > 1) {
                        moveRow(PREV_ROW, currentContainerDiv);
                        return false;
                    }
                }
            }
        } else if (evt.keyCode == KEY.DOWN) {
            if ($(focused).closest('table').attr('class') == CLASS_INPUT_ROW_TABLE) {
                if ($(focused).get(0).tagName != 'SELECT' || evt.ctrlKey) {
                    if (curRowNo < mainTableLength - 2) {
                        moveRow(NEXT_ROW, currentContainerDiv);
                        return false;
                    }
                }
            }
        }

        //console.log('row_no on keyDown', row_no);
    });

    function moveRow(moveNextPrev, $containerDiv) {
        //console.log('moveNextPrev', moveNextPrev);
//        console.log('MoveRow: Cur Row No', curRowNo);
//        console.log('MoveRow: Table Length', mainTableLength);
        if (typeof $containerDiv == typeof undefined || $containerDiv == false){
            $('.' + CLASS_INPUT_ROW_TABLE).hide();
            return ;
        }

//        console.log('moveRow: containerDivId', $containerDiv.attr('id'));
        var isNewRow = false;
        var $mainTable = $containerDiv.find('.' + CLASS_TABLE_ENTRY_GRID);
//        console.log('moveRow: mainTable id', $mainTable.attr('id'));
        var $inputRowTable = $containerDiv.find('.' + CLASS_INPUT_ROW_TABLE);

//        console.log('moveRow: inputRowTable id', $inputRowTable.attr('id'));
        var mainTableLength = $mainTable.find('tr').length;

        if ($inputRowTable.css('display') != 'none') {
            saveValuesToTableRow($containerDiv, $mainTable, $inputRowTable);
        }
        if (moveNextPrev != DONT_MOVE) {
            if (moveNextPrev == NEXT_ROW) {
//                console.log('moveRow: curRowNo mainTableLength - 2', curRowNo + " " + (mainTableLength - 2));
                if (curRowNo == mainTableLength - 2 && allowAddNewRow){
                    addRowToMainTable(null, $mainTable, $inputRowTable);
                    isNewRow = true;
                }
                if (curRowNo < mainTableLength - 1) {
                    curRowNo = curRowNo + 1;
                } else {
                    return false;
                }
            } else if (moveNextPrev == PREV_ROW) {
                if (curRowNo > 1) {
                    curRowNo = curRowNo - 1;
                } else {
                    return false;
                }
            }

            showControls(curRowNo, 1, isNewRow, $containerDiv, $mainTable, $inputRowTable);
        }else{
            showControls(0,0, isNewRow, $containerDiv, $mainTable, $inputRowTable);
        }
        //console.log('curRowNo after', curRowNo);
    }


    function nextOnTabIndex(element) {
        var fields = $($document
            .find('a[href], button, input, select, textarea')
            .filter(':visible').filter(':enabled')
            .toArray()
            .sort(function(a, b) {
                return ((a.tabIndex > 0) ? a.tabIndex : 1000) - ((b.tabIndex > 0) ? b.tabIndex : 1000);
            }));


        return fields.eq((fields.index(element) + 1) % fields.length);
    }


    function addRowToMainTable(jsonRowData, $mainTable, $inputRowTable){
        //$mainTable.append('<tr><td><input name="input1"/></td><td><input name="input1"/> </td><td><input name="input1" class="last"/></td></tr>');

        var containerDivId = $mainTable.attr('containerDiv');
        mainTableLength = $mainTable.find('tr').length;

        var inputRow = $inputRowTable.find("tr").clone();
        var addColumnForButtons = false;
        var buttonsColumn = $("<td>");

        inputRow.css('height', $inputRowTable.height());

        

//        console.log('addRowToMainTable: inputRow No of columns', inputRow.children(0).length);
//        inputRow.css("height", "40px");
//        inputRow.css("border", "1px solid #C0D0E5");
        inputRow.attr("id", containerDivId + "_inputRow_"+ mainTableLength);

        inputRow.children(0).each(function() {
//            console.log('addRowToMainTable: column id', $(this).attr('id'));
//            console.log('addRowToMainTable: column class', $(this).attr('class'));
//            console.log('addRowToMainTable: column textJsonFld', $(this).attr(TEXT_JSON_FLD));
//            console.log('addRowToMainTable: column valJsonFld', $(this).attr(VAL_JSON_FLD));

            var addReadOnlyLabel = false;
            var columnId = containerDivId + '_' + $(this).attr('id');
            var colLabel = $('<div style="width:95%; height:95%; text-overflow: ellipsis; overflow:hidden; white-space: nowrap;" >');
            colLabel.attr('id', 'label_' + columnId + '_' + mainTableLength);

            var colHiddenLabel = null;
            var colImageLabel = null;
            var addHiddenLabel = false;
            var addImageLabel = false;

            var controlWidthPerc = '95%';
            var readOnlyLabelWidthPerc = '0%';
            $(this).attr('id', columnId + "_" + mainTableLength);
            var controlObj = getControlTypeForColumn($(this));
            var $readOnlyLabel = $(this).find('.' + CLASS_READ_ONLY_LABEL );
            if ($readOnlyLabel.length > 0){
                addReadOnlyLabel = true;
                $readOnlyLabel.attr('id', 'label_readOnly_' + columnId + '_' + mainTableLength);
                $readOnlyLabel.html('');
                if (controlObj != null){
                    controlWidthPerc = controlObj.control.css('width');
                    readOnlyLabelWidthPerc = $readOnlyLabel.css('width');

                }
            }
            if (controlObj != null) {
                colLabel.css('text-align', controlObj.control.css('text-align'));
                if (controlObj.controlType == 'SELECT' || controlObj.controlType == 'SELECT2' || controlObj.controlType == 'SELECT2SEARCH' || controlObj.controlType == 'SELECT2NEW') {
                    colHiddenLabel = $('<div style="display: none; width:95%; height:95%; text-overflow: ellipsis; overflow:hidden; white-space: nowrap;">');
                    colHiddenLabel.attr('id', 'label_hidden_' + columnId + '_' + mainTableLength);
                    addHiddenLabel = true;
                }
                if (controlObj.controlType == 'CHECKBOX') {
//                    console.log('addRowToMainTable: controlType', 'checkbox');
                    colHiddenLabel = $('<div style="display: none; width:95%; height:95%; text-overflow: ellipsis; overflow:hidden; white-space: nowrap;">');
                    colHiddenLabel.attr('id', 'label_hidden_' + columnId + '_' + mainTableLength);
                    colImageLabel = $('<div>');
                    colImageLabel.attr('id', 'label_image_' + columnId + '_' + mainTableLength);
                    addHiddenLabel = true;
                    addImageLabel = true;
                }
            }

            if (jsonRowData != null) {
                //if (jsonRowData.ActionFlag == NO_ACTION)
                    FillJsonDataToTable(this, colLabel, colHiddenLabel, colImageLabel, $readOnlyLabel, jsonRowData, addHiddenLabel, addImageLabel, addReadOnlyLabel, controlObj, $mainTable);

            }

            if ($(this).attr('class') == CLASS_BUTTON_COLUMN) {
                $(this).css("display", "");
                addColumnForButtons = true;
//                console.log('addRowToMainTable: buttonColumn html', $(this).html());
                var $colButton = $(this).clone();
                $colButton.find('button').each(function(){
//                    //console.log('addRowToMainTable: ColButton clone child', $(this));
                    var controlType = $(this).get(0).tagName;
                    if (controlType == 'BUTTON'){
                        $(this).attr('id', $(this).attr('id') + '_' + containerDivId + '_' + mainTableLength);
                    }
                });
                buttonsColumn.append($colButton.html());

                $(this).empty();
                $(this).remove();
            }else if($(this).attr('class') == CLASS_HIDDEN_COLUMN){
                $(this).empty();
                $(this).append(colLabel);
            }else{
                $(this).empty();
                if (addReadOnlyLabel){
                    colLabel.css('float', 'left');
                    colLabel.css('width', controlWidthPerc);
                }
                $(this).append(colLabel);
                if (addReadOnlyLabel){
                    $readOnlyLabel.css('overflow', 'hidden');
                    $readOnlyLabel.css('float', 'right');
                    $readOnlyLabel.css('width', readOnlyLabelWidthPerc);
                    $(this).append($readOnlyLabel);
                }
                if (addHiddenLabel){
                    $(this).append(colHiddenLabel);
                }
                if (addImageLabel){
                    $(this).append(colImageLabel);
                }
                $(this).css("border-right", "1px solid #C0D0E5");
                $(this).css("border-bottom", "1px solid #C0D0E5");
            }


//            console.log('addRowToMainTable: column textJsonFld after process', $(this).attr('textJsonFld'));
//            console.log('addRowToMainTable: column valJsonFld after process', $(this).attr('valJsonFld'));
        });


        if (addColumnForButtons || addColumnForDeleteButton){

            if (addColumnForDeleteButton) {
                buttonsColumn.css('width', DELETE_COLUMN_WIDTH);
                buttonsColumn.attr('id', 'colDelete_' + containerDivId + '_' + mainTableLength);

                var buttonDelete = $('<button>');
                buttonDelete.attr('id', 'btnDelete_' + containerDivId + '_' + mainTableLength);
                //console.log('resourcesPath', 'url("' + resourcesPath  + 'glyphicons-halflings.png")');
                if (resourcesPath == ''){
                    resourcesPath = getOptionFromMainTable($mainTable, 'resourcesPath');
                }
                buttonDelete.css({
                    'background': 'url("' + resourcesPath  + 'glyphicons-halflings.png")',
                    'height': '24px',
                    'width': '24px',
                    'background-color': 'rgb(255, 255, 255)',
                    'border': '0px none rgb(255, 255, 255)',
                    'background-position': '24px 0px',
                    'margin-top': '9px'

                });
                buttonDelete.attr('title', 'Remove this row');
                buttonDelete.attr('type', 'button');
                buttonDelete.attr('class', CLASS_DELETE_ROW_BUTTON + " " + CLASS_INPUT_ROW_BUTTON);
                buttonsColumn.append(buttonDelete);
            }
            buttonsColumn.attr('class', CLASS_BUTTONS_COLUMN);
            buttonsColumn.attr('id', containerDivId + '_' + CLASS_BUTTONS_COLUMN + "_" + mainTableLength);
            inputRow.append(buttonsColumn);
        }



        inputRow.appendTo($mainTable);
    }

    function deleteRowFromMainTable($tableRow, $containerDiv, $mainTable, $inputRowTable){
        //TODO: delete row from table
        //change the background color to red before removing
        var bAddEmptyRow = false;
        var rowNo = $tableRow.index();

        $containerDiv.trigger('deleteRow', rowNo);

        console.log('deleteRowFromMainTable', $mainTable.find('tr').length - 1);
        console.log('deleteRowFromMainTable row index', $tableRow.index());

        if ($tableRow.index() == $mainTable.find('tr').length - 1){ //Skip last row
            return;
        }
        if ($tableRow.index() == $mainTable.find('tr').length - 2 && $mainTable.find('tr').length - 1 <= 2){ //replace second last row with blank row
            bAddEmptyRow = true;
        }

        $tableRow.css("background-color","#FF3700");

        var primaryKeyValueForRow = '';
        var primaryKeyJsonFld = $containerDiv.EntryGrid('instance').options.primaryKeyJsonFld;

        var primaryValueColId = $tableRow.find('td[valJsonFld="' + primaryKeyJsonFld + '"]').attr('id');
        primaryKeyValueForRow = $("#label_" + primaryValueColId).html();
        console.log("deleteRowFromMainTable: primaryKeyValueForRow", rowNo);

        $tableRow.fadeOut(400, function(){
            $tableRow.remove();
        });

        shiftRowsOnDelete($mainTable);


        deleteRowFromJson($mainTable, primaryKeyValueForRow);

        if (bAddEmptyRow){
            window.setTimeout(addRowToMainTable(null, $mainTable, $inputRowTable),400);
        }

        $containerDiv.trigger('entrygridrowdeleted', primaryKeyValueForRow);
        
    }

    function shiftRowsOnDelete($mainTable){
        var mainTableRows = $mainTable.find('tr');
        var containerDivId = $mainTable.attr('containerDiv');
        var shiftRow = false;

        mainTableRows.each(function(){
            if ($(this).index() == curRowNo){
                shiftRow = true;
            }
            if (shiftRow){
                var curRowIndex = $(this).index();
                curRowIndex--;
                $(this).attr('id', containerDivId + '_' + getIdWithoutRowNo($(this).attr('id')) + "_" + curRowIndex);
                var mainRowColumns = $(this).find('td');
                mainRowColumns.each(function(){
                    $(this).attr('id', containerDivId + '_' + getIdWithoutRowNo($(this).attr('id')) + "_" + curRowIndex);
                    $(this).children().each(function(){
                        $(this).attr('id', 'label_' + getIdWithoutRowNo($(this).attr('id')) + "_" + curRowIndex);
                    });
                });
            }
        });
    }

    function createTableHeader($mainTable, $inputRowTable, addColumnForDeleteButton){
        var addButtonsColumn = false;
        var buttonCount = 0;

        var inputRow = $inputRowTable.find("tr").clone();

        if (tableHeaderStyle != null && tableHeaderStyle != '') {
            //inputRow.addClass(tableHeaderStyle);
        } else {
//            inputRow.css("background-color", "#f5f5f5");
//            inputRow.css("color", "#000000");
//            inputRow.css("border-bottom", "1px solid #C0D0E5");
            inputRow.css("height", "40px");
            inputRow.css("line-height", "0");

        }

//        console.log('inputRow No of columns', inputRow.children(0).length);
        inputRow.children(0).each(function(){
            $(this).attr('id', $mainTable.attr('containerDiv') + "_" + $(this).attr('id') + "_0");
            $(this).empty();
            if (tableHeaderStyle != null && tableHeaderStyle != '') {
                $(this).addClass(tableHeaderStyle);
            }else {
                $(this).css("background-color", "#f5f5f5");
                $(this).css("color", "#000000");
                $(this).css("border-bottom", "1px solid #C0D0E5");
                $(this).css("border-right", "1px solid #C0D0E5");
            }
            $(this).html($(this).attr('name'));
            if ($(this).hasClass(CLASS_BUTTON_COLUMN)) {
                addButtonsColumn = true;
                buttonCount++;
                $(this).remove();
            }
        });
        if (addColumnForDeleteButton || addButtonsColumn){
            if (addColumnForDeleteButton)
                buttonCount++;
            var buttonsColumn = $("<td>");
            console.log('buttonCount');
            buttonsColumn.css('width', (30 * buttonCount) + 'px');
            inputRow.append(buttonsColumn);

//            inputRow.find('td').eq(n).after(deleteColumn);
//            inputRow.find('td last').after(deleteColumn);
//            deleteColumn.appendTo(inputRow);
        }
        inputRow.attr('id', 'tableEntryGridHeader');
        inputRow.appendTo($mainTable);

    }

    function showControls(row_no, col_no, isNewRow, $containerDiv, $mainTable, $inputRowTable) {

        var primaryKeyJsonFld='';

        var options = $containerDiv.EntryGrid('instance').options;
        var jsonData = options.jsonData;

        primaryKeyJsonFld = options.primaryKeyJsonFld;

        if ($inputRowTable == null)
            return false;
        if (row_no == 0){
            $inputRowTable.hide();
            return false;
        }

        var containerDivId = $containerDiv.attr('id');
//        console.log('showControls: containerDiv', containerDivId);
//        console.log('showControls: rowNo', row_no);
//        console.log('ShowControls: Current Row Id', '#' + containerDivId + '_inputRow_' + row_no);

        $mainTable.trigger('entrygridrowchange', row_no);

        $inputRowTable.find('.buttonColumn').css('display','none');
//        console.log('ShowControls: Current Row Id', '#' + containerDivId + '_inputRow_' + row_no);
        adjustColumnWidthsOnInputGrid($mainTable, $inputRowTable);
        console.log($containerDiv.parent().scrollTop());
        var currRowPosition = $('#' + containerDivId + '_inputRow_' + row_no).position();
        var trHeight = $('#' + containerDivId + '_inputRow_' + row_no).css('height');
//        console.log('ShowControls: Current Row Id', '#' + containerDivId + '_inputRow_' + row_no);
        $inputRowTable.css("top", currRowPosition.top + parseInt($containerDiv.parent().scrollTop()));
        //$inputRowTable.css("left", currRowPosition.left);
        $inputRowTable.css("right", currRowPosition.right);
//        $inputRowTable.css("height", trHeight);
        $inputRowTable.css("display", "block");
        $inputRowTable.css("visibility","visible");
        $inputRowTable.attr('curRowNo', row_no);

        LoadValuesToInputRow($containerDiv, $inputRowTable);

        var controlObj = getPrimaryFldControlObj($containerDiv, primaryKeyJsonFld);
//        console.log('showControls: control name', controlObj);
        if (controlObj.controlType == 'DIV'){
//            console.log('showControls: controlObj.control.html()', controlObj.control.html());
            if (controlObj.control.html() == ''){
                isNewRow = true;
            }
        }

        var setGuidAsPrimaryKey = false;
//        console.log('showControls: newRow', newRow);
        if (isNewRow){
            if (jsonData != null) {
                if (jQuery.Guid.Empty() == jsonData[0][primaryKeyJsonFld] || jQuery.Guid.IsValid(jsonData[0][primaryKeyJsonFld])) {
                    if (controlObj.controlType == 'DIV') {
                        controlObj.control.html(jQuery.Guid.New());
                        setGuidAsPrimaryKey = true;
                    }
                }
            }
            if (setGuidAsPrimaryKey == false){
                newRowPrimaryKeySeedValue++;
                if (controlObj.controlType == 'DIV') {
                    controlObj.control.html(newRowPrimaryKeySeedValue);
                }
            }
        }

        $mainTable.trigger('onRowClick', row_no);
    }

    function getPrimaryFldControlObj($containerDiv, primaryKeyJsonFld){
        var primaryFldColId = $containerDiv.find('td[valJsonFld="' + primaryKeyJsonFld + '"]').attr('id');
        var $primaryFldCol = $("#" + primaryFldColId);
        var controlObj = getControlTypeForColumn($primaryFldCol);
        return controlObj;
    }

    function saveValuesToTableRow($containerDiv, $mainTable, $inputRowTable){
        var primaryKeyJsonValue='';
        var options = $containerDiv.EntryGrid('instance').options;

        var controls = $inputRowTable.find('tr');
        controls.children(0).each(function(){
//            console.log('saveValuesToTableRow: class', $(this).attr('class'));
            if ($(this).attr('class') != CLASS_BUTTON_COLUMN) {
                var control = getControlValueForColumn($(this));
                if (control != null){
                    var columnId = $containerDiv.attr('id') + '_' + $(this).attr('id');
                    var $readOnlyLabelInputRow = $(this).find('.' + CLASS_READ_ONLY_LABEL);
                    if ($readOnlyLabelInputRow.length > 0){
                        $("#label_readOnly_" + columnId + "_" + curRowNo).html($readOnlyLabelInputRow.html());
                        $("#label_readOnly_" + columnId + "_" + curRowNo).attr('title', $readOnlyLabelInputRow.html());
                    }

                    $("#label_" + columnId + "_" + curRowNo).html(control.text);
                    $("#label_" + columnId + "_" + curRowNo).attr('title', control.text);

                    if ($("#label_hidden_" + columnId + "_" + curRowNo).length > 0)
                        $("#label_hidden_" + columnId + "_" + curRowNo).html(control.val);

                    if (control.text == '' && control.controlType == 'CHECKBOX'){
                        if (control.val == 'true' || control.val == true){
                            $("#label_" + columnId + "_" + curRowNo).css('display','none');
                            setCheckboxValue($mainTable, $("#label_image_" + columnId + "_" + curRowNo), true);
                        }else if (control.val == 'false' || control.val == false){
                            $("#label_" + columnId + "_" + curRowNo).css('display','none');
                            setCheckboxValue($mainTable, $("#label_image_" + columnId + "_" + curRowNo), false);
                        }
                    }

                    if ($(this).attr(VAL_JSON_FLD) == options.primaryKeyJsonFld){
                        primaryKeyJsonValue = control.val;
                    }

                }
            }
        });



        saveValuesToJsonData($mainTable, primaryKeyJsonValue, controls);


    }

    function LoadValuesToInputRow($containerDiv, $inputRowTable){
//        console.log('LoadValuesToInputRow: started');
        var controls = $inputRowTable.find('tr');
        controls.children(0).each(function(){
            var controlObj = getControlTypeForColumn($(this));
            var $readOnlyLabelInputRow = $(this).find('.' + CLASS_READ_ONLY_LABEL);
            var readOnlyPresent = false;
            if ($readOnlyLabelInputRow.length > 0){
                readOnlyPresent = true;
            }
            if (controlObj != null) {
                var controlType = controlObj.controlType;

                var control = controlObj.control;
                var controlVal;
                var columnId = $containerDiv.attr('id') + '_' + $(this).attr('id');
                if (readOnlyPresent){
                    $readOnlyLabelInputRow.html( $('#label_readOnly_' + columnId + '_' + curRowNo).html());
                }

//                console.log('LoadValuesToInputRow: control id ',$(control).attr('id') + "," + controlType);
//                console.log('LoadValuesToInputRow: label id ',$("#label_" + columnId + "_" + curRowNo).attr('id'));
//                console.log('LoadValuesToInputRow: label value ',$("#label_" + columnId + "_" + curRowNo).html());
//                console.log('LoadValuesToInputRow: label hidden value ',$("#label_hidden_" + columnId + "_" + curRowNo).html());
//                console.log('LoadValuesToInputRow: control Type ', controlType);
                if (controlType == 'SELECT') {
                    controlVal = $("#label_hidden_" + columnId + "_" + curRowNo).html();
                    control.val(controlVal);
                } else if (controlType == 'INPUT') {
                    controlVal = $("#label_" + columnId + "_" + curRowNo).html();
                    control.val(controlVal);
                }else if (controlType == 'CHECKBOX') {
                    if ($("#label_hidden_" + columnId + "_" + curRowNo).html() == 'true') {
                        control.prop('checked', true);
                    } else {
                        control.prop('checked', false);
                    }
                } else if (controlType == 'SELECT2') { //For Select2 Combos
                    controlVal = $("#label_hidden_" + columnId + "_" + curRowNo).html();
                    if (controlVal == '')
                        controlVal = null;
                    control.select2('val', controlVal);
                } else if (controlType == 'DIV'){
//                    console.log('LoadValuesToInputRow: controlType == DIV');
//                    console.log('LoadValuesToInputRow: control.html', $("#label_" + columnId + "_" + curRowNo).html());
//                    console.log('LoadValuesToInputRow: control.id', "#label_" + columnId + "_" + curRowNo);
                    control.html($("#label_" + columnId + "_" + curRowNo).html());
                } else if (controlType == 'SELECT2SEARCH') {
                    var coltextVal = $('#label_' + columnId + '_' + curRowNo).html();
                    controlVal = $("#label_hidden_" + columnId + "_" + curRowNo).html();
                    $('#' + control.attr('id')).html('<option selected="selected" value=""></option><option selected="selected" value="' + controlVal + '">' + coltextVal + '</option>');
                    $('#select2-' + control.attr('id') + '-container').html(coltextVal);
                } else if (controlType == 'SELECT2NEW') {
                    controlVal = $("#label_hidden_" + columnId + "_" + curRowNo).html();
                    if (controlVal == '')
                        controlVal = null;
                    control.val(controlVal);
                }
            }
        });
    }

    function getControlValueForColumn($column){
//        console.log('getControlValueForColumn: started');

        var controlObjType = getControlTypeForColumn($column);
        if (controlObjType != null) {
            var control = controlObjType.control;
            var controlType = controlObjType.controlType;

            //            console.log('getControlValueForColumn: Control', control.attr('id'));
            //            console.log('getControlValueForColumn: ControlType', controlType);
            if (controlType == 'SELECT') {
                var selectText = $('#' + control.attr('id') + " option:selected").text();
                return { 'text': selectText, 'val': $('#' + control.attr('id') + " option:selected").val() };
            } else if (controlType == 'INPUT') {
                return {'text': control.val(), 'val': control.val(), 'controlType': controlType};
            } else if (controlType == 'CHECKBOX'){
                return {'text': '', 'val': control.prop('checked'), 'controlType': controlType};
            } else if (controlType == 'SELECT2') {
                var text;
                var val;
                if ($("#" + control.attr('id')).select2("data") == null) {
                    text = '';
                    val = '';
                } else {
                    text = $("#" + control.attr('id')).select2("data").text;
                    val = $("#" + control.attr('id')).select2("data").id;
                    if (typeof text == typeof undefined || text == false){
                        text = $("#" + control.attr('id')).select2("data")[$column.attr('select2TextFld')];
                        console.log('getControlValueForColumn: formatSelection', $("#" + control.attr('id')).select2('prepareOpts'));
                    }
                }
                return {'text': text, 'val': val, 'controlType': controlType};
            } else if (controlType == 'DIV'){
                return {'text': control.html(), 'val': control.html(), 'controlType': controlType};
            } else if (controlType == 'SELECT2SEARCH') {
                var select2SearchText = $('#select2-' + control.attr('id') + '-container').html();
                return { 'text': select2SearchText, 'val': control.val() };
            } else if (controlType == 'SELECT2NEW') {
                var select2SearchText = $('#select2-' + control.attr('id') + '-container').html();
                return { 'text': select2SearchText, 'val': control.val() };
            }
        }
        return null;
    }

    function getControlTypeForColumn($column){
//        console.log('getControlTypeForColumn: started');
        var colClass = CLASS_INPUT_ROW_CONTROL;

        if ($column.attr('class') == CLASS_HIDDEN_COLUMN){
            colClass = CLASS_INPUT_ROW_HIDDEN;
        }
        if ($column.attr('class') == CLASS_BUTTONS_COLUMN){
            colClass = CLASS_INPUT_ROW_BUTTON;
        }

//        console.log('getControlTypeForColumn: searched class', colClass);
//        console.log('getControlTypeForColumn: column id', $($column).attr('id'));

        var controls = $column.find('.' + colClass);
        var control = null;
//        console.log('getControlTypeForColumn: controls.length', controls.length);

        if (controls.length > 0){
            control = controls.first();
            var controlType = control.get(0).tagName;
            if (controlType == 'SELECT') {
                for (var i = 0; i < control.get(0).classList.length; i++) {
                    if (control.get(0).classList[i].toLowerCase() == 'select2search') {
                        controlType = 'SELECT2SEARCH';
                    }
                    if (control.get(0).classList[i].toLowerCase() == 'select2new') {
                        controlType = 'SELECT2NEW';
                    }
                }
            }else if(controlType == 'INPUT') {
                if (control.attr('type') == 'checkbox'){
                    controlType = 'CHECKBOX';
                }
            }else if(controlType == 'DIV' && control.attr('id').slice(0,4) == SELECT2_ID_PREFIX && controls.length > 1) { //For Select2 Combos
                control = controls.next();
                controlType = 'SELECT2';
            }else if(controlType == 'DIV'){
                controlType = 'DIV';
            }else if (controlType == 'BUTTON'){
                controlType == 'BUTTON';
            }
            return {'control': control, 'controlType': controlType};
        }
//        console.log('getControlTypeForColumn: Nothing Found');
        return null;
    }

    function setFocusOnColumnControl($col, $inputRowTable){
//        console.log('setFocusOnColumnControl: Nothing Found');

        var inputRowColumnId = getIdWithoutRowNo($col.attr('id'));
//        console.log('setFocusOnColumnControl: column id', inputRowColumnId);
        var $inputColumn = $inputRowTable.find("#" + inputRowColumnId);
//        console.log('setFocusOnColumnControl: found column id', $inputColumn.attr('id'));
        var controlObj = getControlTypeForColumn($inputColumn);


        if (controlObj != null) {
//            console.log('setFocusOnColumnControl: Controls Count', controlObj.length);
            var controlType = controlObj.controlType;
            var control = controlObj.control;

            if (controlType == 'SELECT') {
                control.focus();
            } else if (controlType == 'INPUT') {
                control.focus();
                control.select();
            } else if (controlType == 'SELECT2') { //For Select2 Combos
                control.select2('focus');
            }
        }
    }

    function adjustColumnWidthsOnInputGrid($mainTable, $inputRowTable){
//        console.log('adjustColumnWidthsOnInputGrid: true');
        var mainTableColumns = $mainTable.find('tr:last td');
//        console.log('adjustColumnWidthsOnInputGrid: mainTableColumns', mainTableColumns);
        var totalWidth = parseInt($mainTable.css('width'),10);

//        console.log('adjustColumnWidthsOnInputGrid: totalWidth of main table in beginning', totalWidth);

        mainTableColumns.each(function(){

//            console.log('adjustColumnWidthsOnInputGrid: this.id', $(this).attr('id'));
            var colId = getIdWithoutRowNo($(this).attr('id'));
//            console.log('adjustColumnWidthsOnInputGrid: colId', colId);
            var colWidth = $(this).css('width');
            var inputRowColumn = $inputRowTable.find('#' + colId);
//            console.log('adjustColumnWidthsOnInputGrid: column id & length', colId + "-" + inputRowColumn.length);
            if (inputRowColumn.length == 0 || inputRowColumn.attr('class') == CLASS_BUTTON_COLUMN) {
//                console.log('adjustColumnWidthsOnInputGrid: column Class', inputRowColumn.attr('class'));
//                console.log('adjustColumnWidthsOnInputGrid: column Width', colWidth);
//                if (inputRowColumn.attr('class') == CLASS_BUTTON_COLUMN){
//                    inputRowColumn.css('display','inline');
//                    colWidth = inputRowColumn.css('width');
//                }
//                console.log('adjustColumnWidthsOnInputGrid: column Width', colWidth);
                totalWidth = totalWidth - parseInt(colWidth, 10);
            }
        });
//        console.log('adjustColumnWidthsOnInputGrid: totalWidth', totalWidth);
        $inputRowTable.css('width', totalWidth + 'px');
        mainTableColumns.each(function(){
            var colId = getIdWithoutRowNo($(this).attr('id'));
//            console.log('adjustColumnWidthsOnInputGrid: Column ', colId);
            var colWidth = $(this).css('width');
//            console.log('adjustColumnWidthsOnInputGrid: Column Width', colWidth);
            var inputRowColumn = $inputRowTable.find('#' + colId);
//            var $readOnlyLabelMain = $(this).find('.' + CLASS_READ_ONLY_LABEL);
//            var readOnlyPresent = false;
//            var readOnlyWidth = 0;
//            if ($readOnlyLabelMain.length > 0){
//                readOnlyPresent = true;
//                readOnlyWidth = $readOnlyLabelMain.css('width');
//            }
            if (inputRowColumn.length > 0) {
                inputRowColumn.css('width', parseInt(colWidth,10) + 1 + 'px' );
//                //totalWidth = totalWidth + parseInt(colWidth, 10);
//                var controlObj = getControlTypeForColumn(inputRowColumn);
//                if (!readOnlyPresent) {
//                    if (controlObj != null) {
//                        if (controlObj.controlType == 'SELECT') {
//                            controlObj.control.css('width', .95 * parseInt(colWidth, 10) + 'px');
//                        } else if (controlObj.controlType == 'SELECT2') {
//                            controlObj.control.css('width', .98 * parseInt(colWidth, 10) + 'px');
//                        } else if (controlObj.controlType == 'INPUT') {
//                            controlObj.control.css('width', .90 * parseInt(colWidth, 10) + 'px');
//                        } else if (controlObj.controlType == 'CHECKBOX') {
//                            controlObj.control.css('width', .90 * parseInt(colWidth, 10) + 'px');
//                        }
//                    }
//                }
            }
        });
//        console.log('adjustColumnWidthsOnInputGrid: main Table Width', $mainTable.css('width'));
//        console.log('adjustColumnWidthsOnInputGrid: input Table Width', totalWidth);

    }

    function getColumnJsonFlds($col){
        var attrTextJsonFld = $($col).attr(TEXT_JSON_FLD);
        var attrValJsonFld = $($col).attr(VAL_JSON_FLD);
        var textJsonFld = '';
        var valJsonFld = '';

        if (typeof attrTextJsonFld !== typeof undefined && attrTextJsonFld !== false) {
            textJsonFld = $($col).attr(TEXT_JSON_FLD);
        }

        if (typeof attrValJsonFld !== typeof undefined && attrValJsonFld !== false) {
            valJsonFld = $($col).attr(VAL_JSON_FLD);
        }

        return {textJsonFld: textJsonFld, valJsonFld: valJsonFld};
    }

    function FillJsonDataToTable($col, $control, $controlHidden, $colImageLabel, $readOnlyLabel, jsonRowData, addHiddenLabel, addImageLabel, addReadOnlyLabel, inputRowControlObj, $mainTable){
        var jsonFlds = getColumnJsonFlds($col);
        var textJsonFld = jsonFlds.textJsonFld;
        var valJsonFld = jsonFlds.valJsonFld;



        if (textJsonFld != '') {
            $($control).html(jsonRowData[textJsonFld]);
            $($control).attr('title', jsonRowData[textJsonFld]);
        }else if(valJsonFld != ''){
            $($control).html(jsonRowData[valJsonFld]);
            $($control).attr('title',jsonRowData[valJsonFld]);
        }
        if (addHiddenLabel){
            if (valJsonFld != '') {
                $($controlHidden).html(jsonRowData[valJsonFld]);
            }
        }
        if (addImageLabel){
            $($control).html('');
            $($control).css('display','none');
//            console.log('FillJsonDataToTable:$control',  inputRowControlObj);
            if (inputRowControlObj.control.attr('type') == 'checkbox'){
                setCheckboxValue($mainTable, $colImageLabel, jsonRowData[valJsonFld]);
            }
        }
        if (addReadOnlyLabel){
            var valJsonFldReadOnly = $readOnlyLabel.attr('valJsonFld');
            $readOnlyLabel.html(jsonRowData[valJsonFldReadOnly]);
            $readOnlyLabel.attr('title', jsonRowData[valJsonFldReadOnly]);
        }



    }

    function setCheckboxValue($mainTable, $colImageLabel, checkboxValue){

        var booleanValue = checkboxValue;

        if (resourcesPath == '') {
            resourcesPath = getOptionFromMainTable($mainTable, 'resourcesPath');
        }
        //console.log('setCheckboxValue: options', options);

        $($colImageLabel).css({
            'background': 'url("' + resourcesPath  + 'glyphicons-halflings.png")',
            'height': '24px',
            'width': '24px',
            'background-color': 'rgb(255, 255, 255)',
            'border': '0px none rgb(255, 255, 255)',
            'background-position': '192px 0px',
            'margin-top': '9px'
        });
        if (booleanValue == 'true' || booleanValue == true){
            $($colImageLabel).css('background-position', '192px 0px');
        }else{
            $($colImageLabel).css('background-position', '168px 0px');
        }

    }


    function saveValuesToJsonData($mainTable, primaryKeyValue, controls){
//        console.log("saveValuesToTableRow: jsonData before Save", jsonData);
//        console.log("saveValuesToTableRow: primaryKeyJsonFld", primaryKeyValue);
//        console.log('saveValuesToTableRow: Main options', JSON.parse($mainTable.attr('options')));
//        console.log('saveValuesToJsonData: widget.options', $mainTable.closest('.' + CLASS_TABLE_CONTAINER).EntryGrid('instance').options );

        var options = $mainTable.closest('.'+CLASS_TABLE_CONTAINER).EntryGrid('instance').options;

        var primaryKeyJsonFld = options.primaryKeyJsonFld;
        if (options.jsonData == null || options.primaryKeyJsonFld == '') {
            return false;
        }

        var jsonUpdated = false;
        $.each(options.jsonData, function(index, obj){
//            console.log("saveValuesToJsonData: jsonKey, primaryKey ", obj[primaryKeyJsonFld] + "," +  primaryKeyValue);
            if (obj[primaryKeyJsonFld] == primaryKeyValue){
                fillTableColumnsToJson(controls, obj);
                //console.log("saveValuesToJsonData:- json After Saving", );
                jsonUpdated = true;
                return false;
            }
        });
//        console.log("saveValuesToJsonData: No Entry Found For primaryKey ", primaryKeyValue);
        if (!jsonUpdated) {
            var newRowJson = getNewJsonObject(options.jsonData[0], options.emptyNewJsonRowData);
            fillTableColumnsToJson(controls, newRowJson);
//        console.log('saveValuesToJsonData: newRowJson', newRowJson);
            options.jsonData.push(newRowJson);

        }

        console.log("saveValuesToJsonData: options.jsonData After Saving", $mainTable.closest('.'+CLASS_TABLE_CONTAINER).EntryGrid('instance').options);
        return true;
    }

    function fillTableColumnsToJson(controls, obj) {
        var isEdit = false;
        controls.children(0).each(function(){
            if ($(this).attr('class') != CLASS_BUTTON_COLUMN) {
                var control = getControlValueForColumn($(this));
                var $readOnlyLabelInputRow = $(this).find('.' + CLASS_READ_ONLY_LABEL);
                if ($readOnlyLabelInputRow.length > 0){
                    var valJsonFldReadOnly = $readOnlyLabelInputRow.attr('valJsonFld');
                    obj[valJsonFldReadOnly] = $readOnlyLabelInputRow.html();
                }

                if (control != null) {
                    var columnId = $(this).attr('id');
                    var jsonFlds = getColumnJsonFlds(this);
                    var textJsonFld = jsonFlds.textJsonFld;
                    var valJsonFld = jsonFlds.valJsonFld;

                    if (textJsonFld != '') {
                        //console.log('obj[textJsonFld]  - control.text::', obj[textJsonFld] + ' - ' + control.text);
                        //console.log('obj[textJsonFld] == control.text::', obj[textJsonFld] == control.text);
                        if (obj[textJsonFld] != control.text) {
                            isEdit = true;
                        }
                        obj[textJsonFld] = control.text;
                    }
                    if (valJsonFld != '') {
                        //console.log('obj[valJsonFld] - control.val::', obj[valJsonFld] + ' - ' + control.val);
                        //console.log('obj[valJsonFld] == control.val::', obj[valJsonFld] == control.val);
                        if (obj[valJsonFld] != control.val) {
                            isEdit = true;
                        }
                        if (jQuery.Guid.IsValid(obj[valJsonFld]) && control.val == ''){
                            obj[valJsonFld] = jQuery.Guid.Empty();
                        }else
                            obj[valJsonFld] = control.val;
                    }
                    if (isEdit && obj["ActionFlag"] != ROW_ADDED) {
                        //console.log('ActionFlag::', obj["ActionFlag"]);
                        obj["ActionFlag"] = ROW_EDITED;
                    }
                }
            }
        });
    }

    function deleteRowFromJson($mainTable, primaryKeyValueForRow){


        var options = $mainTable.closest('.'+CLASS_TABLE_CONTAINER).EntryGrid('instance').options;

        if (options.jsonData == null){
            return false;
        }

        var primaryKeyJsonFld = options.primaryKeyJsonFld;
        var databaseKeyJsonFld = options.databaseKeyJsonFld;

        var jsonData2=[];
        var keepRow = true;
        $.each(options.jsonData, function(index, obj){
            keepRow = true;
            if (obj[primaryKeyJsonFld] == primaryKeyValueForRow) {
                if (databaseKeyJsonFld != null && databaseKeyJsonFld != '') {
                    if (obj[databaseKeyJsonFld] == 0 || obj[databaseKeyJsonFld] == '' || jQuery.Guid.IsEmpty(obj[databaseKeyJsonFld])) {
                        keepRow = false;
                    } else {
                        obj.ActionFlag = ROW_DELETED;
                    }
                }
            }
            if (keepRow)
                jsonData2.push(obj);
        });
        options.jsonData = jsonData2;
        console.log('deleteRowFromJson: options.jsonData', $mainTable.closest('.'+CLASS_TABLE_CONTAINER).EntryGrid('instance').options);
    }

    function getNewJsonObject(obj, emptyNewJsonRowData){
        var clone = {};
        for(var i in obj) {


            if(typeof(obj[i])=="object" && obj[i] != null)
                clone[i] = getNewJsonObject(obj[i]);
            else {
                clone[i] = obj[i];
                if (typeof obj[i] != typeof undefined) {
                    //console.log('getNewJsonObject: obj[i] / IsValidGuid', obj[i] + " / " + jQuery.Guid.IsValid(obj[i]));
                    if (jQuery.Guid.IsValid(obj[i])) {
                        clone[i] = jQuery.Guid.Empty();
                    } else {
                        if (emptyNewJsonRowData)
                            clone[i] = '';
                    }
                    //console.log('getNewJsonObject: clone[i]', clone[i]);
                }else{
                    if (emptyNewJsonRowData)
                        clone[i] = '';
                }

            }
        }

        clone.ActionFlag = ROW_ADDED;

        return clone;
    }

    function getIdWithoutRowNo(thisId){
        //console.log('getIdWithoutRowNo: thisId', thisId + ',' + thisId.indexOf("_") + ',' + thisId.lastIndexOf("_"));
        if (typeof thisId != typeof undefined) {
            return thisId.slice(thisId.indexOf("_") + 1, thisId.lastIndexOf("_"));
        }else{
            return
        }
    }

    function convertToGUID(seedValue){
        seedValue = jQuery.Guid.New();
        return seedValue;
    }

    function getOptionFromMainTable($obj, option){
        var $mainTable = null;
        if ($obj.hasClass(CLASS_TABLE_ENTRY_GRID)){
            $mainTable = $obj;
        }else{
            $mainTable = $obj.closest('.' + CLASS_TABLE_ENTRY_GRID);
            if ($mainTable.length == 0){
                return null;
            }
        }

        var options = $mainTable.closest('.'+CLASS_TABLE_CONTAINER).EntryGrid('instance').options;

        if (typeof options != typeof undefined && options != false){
            return options[option];
        }

        return null;
    }

    function validateHTMLAndJSONData($containerDiv, options){
        var allGood = true;
        var inputRowControlCount = 0;

        if ($containerDiv.find('table.' + CLASS_INPUT_ROW_TABLE).length == 0){
            console.error('Container DIV missing table with "inputRowTable" class');
            allGood = false
            return false;
        }

        var $inputRowTable = $containerDiv.find(".inputRowTable" );
        var columns = $inputRowTable.find('td');
        var jsonData = options.jsonData[0];


        if (jsonData != null && jsonData != '') {
            columns.each(function (index, col) {
                var textJsonFld = $(col).attr('textJsonFld');
                var valJsonFld = $(col).attr('valJsonFld');

                if (typeof textJsonFld != typeof undefined && textJsonFld != false) {
                    if (!jsonData.hasOwnProperty(textJsonFld)) {
                        console.error('textJsonFld property missing in JSON Data provided: ', textJsonFld);
                        allGood = false;
                        return false;
                    }
                    if ($containerDiv.find('td[textJsonFld = "' + textJsonFld + '"]').length > 1){
                        console.error('textJsonFld property used more than one column in the Html: ', textJsonFld);
                        allGood = false;
                        return false;
                    }
                }
                if (typeof valJsonFld != typeof undefined && valJsonFld != false) {
                    if (!jsonData.hasOwnProperty(valJsonFld)) {
                        console.error('valJsonFld property missing in JSON Data provided: ', valJsonFld);
                        allGood = false;
                        return false;
                    }
                    if ($containerDiv.find('td[valJsonFld = "' + valJsonFld + '"]').length > 1){
                        console.error('valJsonFld property used more than one column in the Html: ', valJsonFld);
                        allGood = false;
                        return false;
                    }
                }

                if ($(col).find('.' + CLASS_INPUT_ROW_CONTROL).length > 0){
                    inputRowControlCount++;
                }
            });
        }
        if (allGood){
            if (inputRowControlCount == 0){
                console.info('No controls with "InputRowControl" class found. Entry Grid may not behave as expected');
            }
        }

        if (allGood){
            if (options.primaryKeyJsonFld != null && options.primaryKeyJsonFld != ''){
                if (!jsonData.hasOwnProperty(options.primaryKeyJsonFld)){
                    console.error('primaryKeyJsonFld property missing in JSON Data provided: ', options.primaryKeyJsonFld);
                    allGood = false;
                    return false;
                }
            }else{
                console.info('primaryKeyJsonFld missing. No data will be updated to Json');
            }
        }

        if (allGood){
            if (options.databaseKeyJsonFld != null && options.databaseKeyJsonFld != ''){
                if (!jsonData.hasOwnProperty(options.databaseKeyJsonFld)){
                    console.error('databaseKeyJsonFld property missing in JSON Data provided: ', options.databaseKeyJsonFld);
                    allGood = false;
                    return false;
                }
            }else{
                console.info('databaseKeyJsonFld missing. Deletion may be incomplete.');
            }
        }

        if (allGood){
            if (jsonData != null && jsonData != '') {
                if (!jsonData.hasOwnProperty('ActionFlag')) {
                    console.info('Sample JSON Data missing "ActionFlag" property.');
                    allGood = false;
                    return false;
                }
                if (jsonData.ActionFlag != EMPTY_JSON) {
                    console.error('Sample JSON Data missing in the provided JSON.');
                    allGood = false;
                    return false;
                }
            }
        }

        if (allGood)
            return true;
        else
            return false;
    }

//    function generateUUID() {
//        var d = new Date().getTime();
//        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
//            var r = (d + Math.random()*16)%16 | 0;
//            d = Math.floor(d/16);
//            return (c=='x' ? r : (r&0x3|0x8)).toString(16);
//        });
//        return uuid;
//    };

})(jQuery);
