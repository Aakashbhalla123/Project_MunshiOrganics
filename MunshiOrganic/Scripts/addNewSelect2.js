/**
 * Created by VishalGoel on 11/3/2014.
 */
(function($){
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
        F2: 113,
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


   var divAddNewCss={
        backgroundColor: '#f5f5f5',
        width: '100px',
        height: '40px',
        display: 'table-cell',
        'vertical-align': 'middle',
        position: 'absolute',
        top:0,
        left:0
    };

    var SELECT2_ID_PREFIX = 's2id';
    var SELECT2_NO_RESULTS = 'select2-no-results';
    var ADD_NEW_OPTION = 'addNewOption';
    var $parentElement = null;
    var select2Drop = 'select2-drop';
    var _search = '_search';
    var select2InputIdPrefix = 's2id_autogen';
    var onClick = function(){console.log('$divAddNew.click: true');};
    var showAddNew = false;
    var select2 = null;

    var select2Controls = [];

    var prevHeight = 0;

    $.widget("natural.addNewSelect2", {
        options:{
            select2: null,
            onClick: function(){console.log('Send function as parameter to add new');},
            showAddNew : false
        },

        _create: function () {
            //console.log('_create: true', this.options.onClick);
            var currentId = '';
            var localOptions;

            var showAddNew = false;
            this._setOptions({
                select2: this.options.select2,
                onClick : this.options.onClick,
                showAddNew : this.options.showAddNew
            });

            showAddNew = this.options.showAddNew;
            localOptions = this.options;

            $parentElement = this.element;

            //console.log('_Create: select2', select2);

            if (select2 != null){
                $parentElement.select2(select2);
            }else
                $parentElement.select2();

            var $select2ContainerDiv = $('#' + SELECT2_ID_PREFIX + '_' + $($parentElement).attr('id'));
            var s2Index = 0;
//            console.log('_create: container Id', $($select2ContainerDiv).attr('id'));
//            console.log('_create: container Id', SELECT2_ID_PREFIX + '_' + $($parentElement).attr('id'));

            if (typeof undefined != typeof $select2ContainerDiv && $select2ContainerDiv != false){
                currentId = $($select2ContainerDiv).find('input').eq(0).attr('id');

//                console.log('_create: $select2ContainerDiv', $select2ContainerDiv);
//                console.log('_create: $select2ContainerDiv', $($select2ContainerDiv).find('input'));
                s2Index = $($select2ContainerDiv).find('input').eq(0).attr('id').slice(0,select2InputIdPrefix.length + 1);
                s2Index = s2Index[s2Index.length -1];
//                console.log('s2Index', s2Index);
                select2Controls.push({id: currentId, options: this.options, s2Index: s2Index});
//                console.log ("currentId", currentId);
//                console.log('_create: Input for s2id - id ', $($select2ContainerDiv).find('input').eq(0).attr('id'));
//                console.log('_create: Input for s2id - id ', select2Controls);
            }
//
            $($parentElement).on("select2-loaded", function(e) {
                createAddNewDiv(localOptions, s2Index);
//                console.log ("loaded (data property omitted for brevity)", localOptions.showAddNew );

            });

            $(document).keyup(function(e){
//                console.log('keyup: activeElement', $(document.activeElement).attr('id'));
                var controlIndex = findS2Id($(document.activeElement).attr('id'));
//                console.log('controlIndex', controlIndex);
                if (controlIndex > -1){
                    if (select2Controls[controlIndex].options.showAddNew) {
                        if (e.keyCode == KEY.F2) {
                            select2Controls[controlIndex].options.onClick();
                            e.stopPropagation();
                            return false;
                        }

                        var noResults = $('#' + select2Drop).find('.' + SELECT2_NO_RESULTS);
//                        console.log('KeyDown.noResults', noResults);
                        if (noResults.length == 1) {
                            //console.log('KeyDown.noResults', 'true ' + showAddNew);
                            createAddNewDiv(localOptions, select2Controls[controlIndex].s2Index);
                            e.stopPropagation();
                            return false;
                        }
                    }
                }
            });

//            $($divAddNew).click(function(){
//                if (this.options.showAddNew) {
//                    console.log('click', this.options.onClick);
//                    this.options.onClick();
//
//                }
//                return false;
//            });

            //$(this.element).append();
        },

        _setOption: function (key, value) {
            switch (key) {
                case 'onClick':
                    onClick = this.options.onClick;
                    break;
                case 'select2':
                    select2 = this.options.select2;
                    break;
                case 'showAddNew':
                    showAddNew = this.options.showAddNew;
                    break;

            }
        },

        _setOptions: function( options ) {
            this._super(options);
        }
    });



    function findS2Id(s2id){
        var found = false;
        var controlIndex = -1;
        $.each(select2Controls, function(index, select2InputId){
//            console.log('current id, saved id', s2id + ", " + select2InputId.id + ", " + s2id.slice(0,select2InputId.id.length));
            if (s2id.slice(0,select2InputId.id.length) == select2InputId.id){
//                console.log('true');
                found = true;
                controlIndex = index;
                return false;
            }
        });

        return controlIndex;
    }

    function createAddNewDiv(options, s2Index){

        if (!options.showAddNew)
            return false;

//        console.log('createAddNewDiv' , options);

        var $divAddNew = $("<div class='naturalDropDownAddNew select2-drop select2-display-none select2-with-searchbox select2-drop-active' id='" + ADD_NEW_OPTION  + "_" + s2Index + "' >");

        var s2AutogenSearchInputId = select2InputIdPrefix + s2Index + _search;
//        console.log('s2AutogenSearchInputId',s2AutogenSearchInputId);
        var $select2Drop = $('#' + s2AutogenSearchInputId).closest('.' + select2Drop);
//        console.log('$select2Drop',$select2Drop);

        var $addNew = $($select2Drop).find('#' + ADD_NEW_OPTION + "_" + s2Index);
//        console.log('$addNew', $addNew);

        var dropDownHeight = parseInt($select2Drop.css('height'),10);
//        console.log('DropDown Height', dropDownHeight);

        if ($addNew.length > 0){
//            console.log("AddNew Div found");
//                console.log('drop-down height', $select2Drop.css('height'));
            //dropDownHeight = dropDownHeight + 45;
            //$select2Drop.height(dropDownHeight);
            divAddNewCss.top = dropDownHeight + 1;
            divAddNewCss.left = -1;
            $addNew.css(divAddNewCss);
        }else{
//            console.log("Adding New Div");
            $divAddNew.html($('<label id="addNewLabel" style="float:left;padding-left: 10px; padding-top: 5px; width:50%"><span style="font-size: 18px">+</span> Add New</label><label id="keyShortcut" style="float:right; padding-right: 10px;padding-top:5px; width:30%; text-align:right"><span style="font-size: 12px">F2</span></label>'));
//            console.log("_create: div class", $divAddNew.attr('class'));

            divAddNewCss.width = parseInt($select2Drop.css('width'),10);
            divAddNewCss.top = dropDownHeight + 1;
            divAddNewCss.left = -1;
            $divAddNew.css(divAddNewCss);
            $select2Drop.append($divAddNew);
            prevHeight = dropDownHeight;
        }

//        console.log("divAddNew", $divAddNew);

//        console.log('open: drop height', dropDownHeight + "," + prevHeight);

        $select2Drop.css('border-radius',0);


        $($divAddNew).click(function(){
            if (options.showAddNew) {
//                console.log('click', options.onClick);
                options.onClick();

            }
            return false;
        });
    }


})(jQuery);