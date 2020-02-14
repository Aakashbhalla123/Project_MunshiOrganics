/**
 * Created by VishalGoel on 10/18/2014.
 */
(function($){
    var jsonData = null;
    var resourcesPath = '';


    var CLASS_INPUT_ROW_CONTROL_DYNAMIC = 'inputRowControl';
    var CLASS_INPUT_ROW_HIDDEN_DYNAMIC = 'inputRowHidden';

    var COL_FLD_TYPE_NUMERIC = 'NUMERIC';
    var COL_FLD_TYPE_LOV = 'LOV';
    var COL_FLD_TYPE_STRING = 'STRING';
    var COL_FLD_TYPE_DATE = 'DATE';
    var COL_FLD_TYPE_BOOLEAN = 'BOOLEAN';
    var CLASS_TABLE_INPUT_ROW = 'inputRowTable';

    var colCss = {
        padding: '4px',
        'vertical-align': 'middle',
        'text-align': 'left'
    };

    $.widget("natural.CreateInputRowTable", {

        options: {
            resourcesPath:'',
            jsonData:''
        },

        _create: function () {

            this._setOptions({
                resourcesPath: this.option.resourcesPath,
                jsonData: this.option.jsonData

            });

            var $containerDiv = this.element;
            var $inputRowTable = $("<table class='inputRowTable' style='height: 35px; background-color: #fff; color: #000; position: absolute; float: left; width: 98%; border: 2px solid #999999; box-shadow: 0px 0px 8px #888888; border-radius: 3px; table-layout: fixed;' cellspacing='0' >");

            $inputRowTable.attr('id', $containerDiv.attr('id') + '_newRow');

            createColumns($inputRowTable, $containerDiv, jsonData);

            $(this.element).append($inputRowTable);
        },

        _setOption: function (key, value) {
            switch (key) {
                case "resourcesPath":
                    resourcesPath = this.options.resourcesPath;
                    break;
                case "jsonData":
                    jsonData = $.parseJSON(this.options.jsonData);
                    break;


            }
        },

        _setOptions: function( options ) {
            this._super( options );
        },
        destroy: function () {
            $(this.element).find('.' + CLASS_TABLE_INPUT_ROW).remove();
            // Call the base destroy function.
            $.Widget.prototype.destroy.call(this);
        },
        move: function (dx) {
//            var x = dx + parseInt(this._button.css("left"));
//            this._button.css("left", x);
        }
    });

    function createColumns($inputRowTable, $containerDiv, jsonData){
        console.log('createColumns: jsonData', jsonData);
        var containerId = $containerDiv.attr('id') + '_';
        var $tr = $('<tr>');
        $.each(jsonData, function(index, jsonObj){
            console.log('createColumns: jsonObj', jsonObj);
            var $newColumn = $("<td>");
            var $columnControl = null;
            $newColumn.attr('id', containerId + getIdForString(jsonObj.columnName));
            $newColumn.attr('name', jsonObj.columnName);
            $newColumn.attr('txtJsonFld', jsonObj.columnName);
            $newColumn.attr('valJsonFld', jsonObj.columnName);
            $newColumn.css(colCss);
            $newColumn.addClass(jsonObj.columnCssClass);

            if (jsonObj.columnCssClass == 'hiddenColumn'){
                $newColumn.css('display','none');
                $columnControl = $("<div>");
                $columnControl.attr('class', CLASS_INPUT_ROW_HIDDEN_DYNAMIC);
            }else {
                if (jsonObj.columnFldType.toUpperCase() == COL_FLD_TYPE_LOV) {
                    $columnControl = $("<select>");
                    console.log('createColumns: jsonObj.columnValue', jsonObj.columnValue);
                    var values = jsonObj.columnValue.split(',');
                    for (var i = 0; i < values.length; i++) {
                        $($columnControl).append('<option value=' + values[i] + '>' + values[i] + '</option>');
                    }
                } else {
                    $columnControl = $("<input>");
                    if (jsonObj.columnFldType.toUpperCase() == COL_FLD_TYPE_BOOLEAN){
                        $columnControl.prop('type', 'checkbox');
                    }else{
                        $columnControl.prop('type', 'text');
                    }
                }
                $columnControl.attr('class', CLASS_INPUT_ROW_CONTROL_DYNAMIC);
            }
            $columnControl.attr('id', $newColumn.attr('id') + '_Control');
            $columnControl.css('width', '95%');
            if (jsonObj.columnFldType.toUpperCase() == COL_FLD_TYPE_NUMERIC){
                $newColumn.css('text-align', 'right');
                $columnControl.css('text-align', 'right');
            }
            $newColumn.append($columnControl);

            $tr.append($newColumn);
        });
        $inputRowTable.append($tr);
    }

    function getIdForString(columnId){
        return columnId.replace(/ /g , "");
    }

})(jQuery);
