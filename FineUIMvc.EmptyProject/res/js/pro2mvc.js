(function ($) {
    $.fn.getAttributes = function () {
        var attributes = {};

        if (this.length) {
            $.each(this[0].attributes, function (index, attr) {
                attributes[attr.name] = attr.value;
            });
        }

        return attributes;
    };
})(jQuery);

var slashT = '    ';

function UP(ch) {

    ch = ch.toLowerCase();

    ch = ch.charAt(0).toUpperCase() + ch.substr(1);


    if (CHARS[ch]) {
        return CHARS[ch];
    } else {
        return ch;
    }
}

function processAttrs(nodeName, attrs, level) {
    var result = [];
    $.each(attrs, function (cellName, cellValue) {

        // EnableBlurEvent ....
        if (/enable\w+event/ig.test(cellName)) {
            return true; // continue
        }

        // 这些属性不输出
        if ($.inArray(cellName, ['runat', 'enablepostback', 'autopostback']) >= 0) {
            return true; // continue
        }

        cellName = UP(cellName);

        var cellValueLower = cellValue.toLowerCase();

        if (nodeName === 'Toolbar' && cellName === 'Position') {
            cellValue = 'ToolbarPosition.' + cellValue;
        } else if (cellValueLower === 'false' || cellValueLower === 'true') {
            cellValue = cellValueLower;
        } else if ($.inArray(cellName, [
            'OnClick',
            'OnBlur',
            'OnTextChanged',
            'OnTriggerClick',
            'OnTrigger1Click',
            'OnTrigger2Click',
            'OnCheckedChanged',
            'OnSelectedIndexChanged',
            'OnCheckedChanged',
            'OnFileSelected',
            'OnNodeLazyLoad', 'OnNodeExpand', 'OnNodeCollapse',
            'OnTabIndexChanged', 'OnExpand', 'OnCollapse', 'OnClose',
            'OnRowSelect', 'OnPageIndexChanged', 'OnRowLazyLoad', 'OnRowDoubleClick',
            'OnAfterEdit', 'OnFilterChanged'
        ]) >= 0) {
            cellValue = 'Url.Action("' + cellValue + '")';
        } else if ($.inArray(cellName, ['IconUrl', 'ImageUrl', 'BasePath', 'IFrameUrl']) >= 0) {
            cellValue = 'Url.Content("' + cellValue + '")';
        } else if ($.inArray(cellName, ['Width',
            'Height', 'ImageWidth',
            'ImageHeight', 'AutoGrowHeightMin', 'AutoGrowHeightMax', 'BoxFlex',
            'MinHeight', 'MaxHeight', 'MinWidth', 'MaxWidth',
            'MarginRight', 'MarginLeft', 'MarginTop', 'MarginBottom',
            'LabelWidth', 'DecimalPrecision', 'ColumnNumber',
            'ActiveTabIndex', 'ActivePaneIndex',
            'TabSpace', 'RegionSplitWidth', 'ItemSpace',
            'AbsoluteX', 'AbsoluteY', 'TableConfigColumns', 'TableRowspan', 'TableColspan',
            'PageSize', 'ClicksToEdit', 'MinLength', 'MaxLength']) >= 0) {
            cellValue = parseInt(cellValue, 10);
        } else if ($.inArray(cellName, ['MinValue', 'MaxValue', 'Increment']) >= 0) {
            cellValue = parseFloat(cellValue);
        } else if ($.inArray(cellName, [
            'Icon', 'IconFont', 'IconAlign',
            'TextMode',
            'LabelAlign',
            'Editor',
            'TriggerIcon',
            'CompareType',
            'RegexPattern', 'MessageTarget', 'RedStarPosition',
            'ToolbarAlign', 'TabAlign',
            'TabPosition', 'WindowPosition',
            'CloseAction', 'TextAlign', 'SummaryPosition',
            'Target', 'AjaxLoadingType', 'FieldType', 'Renderer',
            'HideMode']) >= 0) {
            cellValue = cellName + '.' + cellValue;
        } else if (cellName === 'Target') {
            if ($.inArray(cellName, ['Self', 'Parent', 'Top']) >= 0) {
                cellValue = cellName + '.' + cellValue;
            } else {
                cellValue = '"' + cellValue + '"';
            }
        } else if (cellName === 'PositionY') {
            cellValue = 'Position.' + cellValue;
        } else if ($.inArray(cellName, ['BodyPadding', "Margin"]) >= 0) {
            cellValue = $.trim(cellValue);
            var cellValueParts = cellValue.split(' ');
            // 仅处理 BodyPadding="5px" 的情况，不处理 BodyPadding="5px 10px" 的情况
            if (cellValueParts.length === 1) {
                cellValue = parseInt(cellValue, 10);
            } else {
                cellValue = '"' + cellValue + '"';
            }
        } else if (cellName === 'PositionX') {
            cellValue = 'Position.' + cellValue;
        } else if (cellName === 'ButtonIconFont') {
            cellValue = 'IconFont.' + cellValue;
        } else if (cellName === 'HeaderTextAlign') {
            cellValue = 'TextAlign.' + cellValue;
        } else if (cellName === 'Type') {
            cellValue = 'ButtonType.' + cellValue;
        } else if (cellName === 'Size') {
            cellValue = 'ButtonSize.' + cellValue;
        } else if (cellName === 'BoxConfigPosition') {
            cellValue = 'BoxLayoutPosition.' + cellValue;
        } else if (cellName === 'BoxConfigAlign') {
            cellValue = 'BoxLayoutAlign.' + cellValue;
        } else if (cellName === 'BoxConfigAlign') {
            cellValue = 'BoxLayoutAlign.' + cellValue;
        } else if (cellName === 'ToolbarSet') {
            cellValue = 'EditorToolbarSet.' + cellValue;
        } else if (cellName === 'CompareOperator') {
            cellValue = 'Operator.' + cellValue;
        } else if ($.inArray(cellName, ['Trigger1Icon', 'Trigger2Icon']) >= 0) {
            cellValue = 'TriggerIcon.' + cellValue;
        } else if (cellName === 'Layout') {
            cellValue = 'LayoutType.' + cellValue;
        } else if (cellName === 'RegionPosition') {
            cellValue = 'Position.' + cellValue;
        } else if (cellName === 'ValidateTarget') {
            cellValue = 'Target.' + cellValue;
        } else if (cellName === 'ConfirmTarget') {
            cellValue = 'Target.' + cellValue;
        } else {
            cellValue = '"' + cellValue + '"';
        }

        result.push(LV(level) + '.' + cellName + '(' + cellValue + ')');
    });
    return result.join('');
}

function LV(level) {
    var result = '\r\n' + slashT;
    for (var i = 0; i < level; i++) {
        result += slashT;
    }
    return result;
}

function processTag(result, source, scriptInside, level) {
    var sourceEl = $(source), sourceElLength = sourceEl.length;

    sourceEl.each(function (index) {
        var node = $(this);
        var nodeName = this.nodeName;

        if (F.startsWith('F:', nodeName)) {
            var nodeName = UP(nodeName.substr('F:'.length));

            if (nodeName === 'PageManager') {
                return true; // continue
            }

            if (nodeName === 'ContentPanel') {
                nodeName = 'Panel';
            }

            // 需要额外的 .Items() 或者 .Nodes()
            var needItemsTag = $.inArray(nodeName, ['Menu', 'ButtonGroup', 'CheckBoxList', 'RadioButtonList', 'DropDownList', 'TreeNode', 'DataList']) >= 0;

            // 表格特殊处理
            if (nodeName === 'BoundField' || nodeName === 'TemplateField') {
                nodeName = 'RenderField';
            } else if (nodeName === 'CheckBoxField') {
                nodeName = 'RenderCheckField';
            }

            if (nodeName === 'Listener') {
                var nodeAttrs = node.getAttributes();
                var nodeAttrEvent = nodeAttrs['event'];
                var nodeAttrHandler = nodeAttrs['handler'];
                result.push(LV(level) + '.Listener("' + nodeAttrEvent + '", "' + nodeAttrHandler + '")');
            } else {
                // F.Tree()
                if (scriptInside) {
                    result.push(LV(level) + 'F.' + nodeName + '()');
                } else {
                    // 需要 AT( ) 标签
                    result.push(LV(level) + '@(F.' + nodeName + '()');
                }

                // 先处理标签的属性
                result.push(processAttrs(nodeName, node.getAttributes(), level + 1));
            }


            var isEmptyItemsTag = false;
            if (needItemsTag) {

                if (nodeName === 'TreeNode') {
                    result.push(LV(level + 1) + '.Nodes(');
                } else {
                    result.push(LV(level + 1) + '.Items(');
                }

                var resultLengthBackup = result.length;
                processTag(result, node.html(), true, level + 2);
                // 如果上面处理中没有添加任何内容，则说明这是个空的 .Items
                if (result.length === resultLengthBackup) {
                    result.pop();
                    isEmptyItemsTag = true;
                }

            } else {

                processTag(result, node.html(), true, level + 1);
            }


            // 如果需要额外的 .Items( ，并且不为空，则添加结束小括号
            if (needItemsTag && !isEmptyItemsTag) {
                result.push(LV(level + 1) + ')');
            }


            // 内部脚本，每一项后面加分号
            if (scriptInside) {
                if (index != sourceElLength - 1) {
                    result.push(',');
                }
            }


            // 需要 AT( ) 标签
            if (!scriptInside) {
                result.push(LV(level) + ')');
            }


        } else if (nodeName === 'MENU') {

            result.push(LV(level) + '.Menu(F.Menu()' + LV(level + 1) + '.Items(');

            processTag(result, node.html(), true, level + 2);

            result.push(LV(level + 1) + ')' + LV(level) + ')');

        } else if (nodeName === 'FILTER') {

            result.push(LV(level) + '.Filter(F.GridFilter()');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'ITEMS') {

            var lastLineText = result[result.length - 1];
            if (F.endsWith('.Items(', lastLineText)) {
                // .Items( 已经存在了
                processTag(result, node.html(), true, level);
            } else {
                result.push(LV(level) + '.Items(');

                processTag(result, node.html(), true, level + 1);

                result.push(LV(level) + ')');
            }


        } else if (nodeName === 'TOOLBARS') {

            result.push(LV(level) + '.Toolbars(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'ROWS') {

            result.push(LV(level) + '.Rows(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'REGIONS') {

            result.push(LV(level) + '.Regions(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'POPPANEL') {

            result.push(LV(level) + '.PopPanel(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'OPERATOR') {

            result.push(LV(level) + '.Operator(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'EDITOR') {

            result.push(LV(level) + '.Editor(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'FIELD') {

            result.push(LV(level) + '.Field(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'COLUMNS') {

            result.push(LV(level) + '.Columns(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'PAGEITEMS') {

            result.push(LV(level) + '.PageItems(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'NODES') {

            result.push(LV(level) + '.Nodes(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'TABS') {

            result.push(LV(level) + '.Tabs(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'TOOLS') {

            result.push(LV(level) + '.Tools(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'PANES') {

            result.push(LV(level) + '.Panes(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'OPTIONS') {

            result.push(LV(level) + '.Options(');

            processTag(result, node.html(), true, level + 1);

            result.push(LV(level) + ')');

        } else if (nodeName === 'LISTENERS') {

            processTag(result, node.html(), true, level);


        } else if (nodeName === '#text') {
            var textContent = this.textContent;
            if (textContent && !scriptInside) {
                result.push(textContent);
            }
        } else {
            result.push(this.outerHTML);
        }
    });

}

$(function () {
    $('#convertit').click(function () {
        var source = $.trim($('#source').val());

        var formStartIndex = source.indexOf('<form ');
        if (formStartIndex >= 0) {
            formStartCloseIndex = source.indexOf('>', formStartIndex) + 1;

            var formEndIndex = source.lastIndexOf('</form>');

            source = source.substr(formStartCloseIndex, formEndIndex - formStartCloseIndex);
        }

        var result = [];

        // source + '<br>'，这样处理可以保证最后的文本信息保留
        processTag(result, source + '<br>', false, 0);

        // 去除最后的 <br>
        var resultstr = result.join('');
        resultstr = resultstr.substr(0, resultstr.length - '<br>'.length);

        $('#output').val(resultstr);
    });



});


var CHARS = {
    'Autocolumnwidth': 'AutoColumnWidth',
    'Trimendzero': 'TrimEndZero',
    'Maxlength': 'MaxLength',
    'Minlength': 'MinLength',
    'Onfilterchange': 'OnFilterChanged',
    'Oncleariconclick': 'OnClearIconClick',
    'Showtime': 'ShowTime',
    'Poppanel': 'PopPanel',
    'Dropdownbox': 'DropDownBox',
    'Treeexpandondblclick': 'TreeExpandOnDblClick',
    'Onrowdoubleclick': 'OnRowDoubleClick',
    'Onrowlazyload': 'OnRowLazyLoad',
    'Onrowselect': 'OnRowSelect',
    'Regionpanel': 'RegionPanel',
    'Datalist': 'DataList',
    'Enableajaxloading': 'EnableAjaxLoading',
    'Ajaxloadingtype': 'AjaxLoadingType',
    'Toolbarset': 'ToolbarSet',
    'Onclose': 'OnClose',
    'Onpageindexchange': 'OnPageIndexChanged',
    'Datakeynames': 'DataKeyNames',
    'Templatefield': 'TemplateField',
    'Renderfield': 'RenderField',
    'Rendercheckfield': 'RenderCheckField',
    'Windowfield': 'WindowField',
    'Rownumberfield': 'RowNumberField',
    'Linkbuttonfield': 'LinkButtonField',
    'Imagefield': 'ImageField',
    'Groupfield': 'GroupField',
    'Hyperlinkfield': 'HyperLinkField',
    'Checkboxfield': 'CheckBoxField',
    'Boundfield': 'BoundField',
    'Datanavigateurlfields': 'DataNavigateUrlFields',
    'Hideonclick': 'HideOnClick',
    'Menutext': 'MenuText',
    'Oncollapse': 'OnCollapse',
    'Onexpand': 'OnExpand',
    'Toolbarfill': 'ToolbarFill',
    'Toolbarseparator': 'ToolbarSeparator',
    'Toolbartext': 'ToolbarText',
    'Menucheckbox': 'MenuCheckBox',
    'Menuseparator': 'MenuSeparator',
    'Regionspliticon': 'RegionSplitIcon',
    'Contentpanel': 'ContentPanel',
    'Selecteddate': 'SelectedDate',
    'Ontabindexchanged': 'OnTabIndexChanged',
    'Accordionpane': 'AccordionPane',
    'Tabstrip': 'TabStrip',
    'Onnodeexpand': 'OnNodeExpand',
    'Onnodecollapse': 'OnNodeCollapse',
    'Onnodelazyload': 'OnNodeLazyLoad',
    'Enabletablestyle': 'EnableTableStyle',
    'Grouppanel': 'GroupPanel',
    'Onfileselected': 'OnFileSelected',
    'Fileupload': 'FileUpload',
    'Dropdownlist': 'DropDownList',
    'Columnwidths': 'ColumnWidths',
    'Formrow': 'FormRow',
    'Oncheckedchanged': 'OnCheckedChanged',
    'Onselectedindexchanged': 'OnSelectedIndexChanged',
    'Checkitem': 'CheckItem',
    'Radioitem': 'RadioItem',
    'Checkbox': 'CheckBox',
    'Checkboxlist': 'CheckBoxList',
    'Radiobutton': 'RadioButton',
    'Radiobuttonlist': 'RadioButtonList',
    'Timepicker': 'TimePicker',
    'Id': 'ID',
    'Iconfont': 'IconFont',
    'Linkbutton': 'LinkButton',
    'Onclientclick': 'OnClientClick',
    'Onclick': 'OnClick',
    'Menuhyperlink': 'MenuHyperLink',
    'Menubutton': 'MenuButton',
    'Navigateurl': 'NavigateUrl',
    'Menuid': 'MenuID',
    'Pagemanager': 'PageManager',
    'Cssclass': 'CssClass',
    'Iconalign': 'IconAlign',
    'Iconurl': 'IconUrl',
    'Buttongroup': 'ButtonGroup',
    'Boxconfigalign': 'BoxConfigAlign',
    'Enablepress': 'EnablePress',
    'Allowmultipress': 'AllowMultiPress',
    'Enablepressgroup': 'EnablePressGroup',
    'Encodetext': 'EncodeText',
    'Hyperlink': 'HyperLink',
    'Focusonpageload': 'FocusOnPageLoad',
    'Layoutafterimagesload': 'LayoutAfterImagesLoad',
    'Autoscroll': 'AutoScroll',
    'Minheight': 'MinHeight',
    'Minwidth': 'MinWidth',
    'Maxheight': 'MaxHeight',
    'Maxwidth': 'MaxWidth',
    'Enableframe': 'EnableFrame',
    'Bodystyle': 'BodyStyle',
    'Bodypadding': 'BodyPadding',
    'Showborder': 'ShowBorder',
    'Isviewport': 'IsViewPort',
    'Customtoolahead': 'CustomToolAhead',
    'Iframeurl': 'IFrameUrl',
    'Iframename': 'IFrameName',
    'Enableiframe': 'EnableIFrame',
    'Enablecollapseevent': 'EnableCollapseEvent',
    'Enableexpandevent': 'EnableExpandEvent',
    'Enablecollapse': 'EnableCollapse',
    'Titlealign': 'TitleAlign',
    'Titletooltip': 'TitleToolTip',
    'Titletooltiptype': 'TitleToolTipType',
    'Showheader': 'ShowHeader',
    'Iconurl': 'IconUrl',
    'Iconfont': 'IconFont',
    'Enableiframe': 'EnableIFrame',
    'Iframeurl': 'IFrameUrl',
    'Iframename': 'IFrameName',
    'Labelwidth': 'LabelWidth',
    'Labelseparator': 'LabelSeparator',
    'Offsetright': 'OffsetRight',
    'Labelalign': 'LabelAlign',
    'Redstarposition': 'RedStarPosition',
    'Messagetarget': 'MessageTarget',
    'Showlabel': 'ShowLabel',
    'Showemptylabel': 'ShowEmptyLabel',
    'Showredstar': 'ShowRedStar',
    'Labelseparator': 'LabelSeparator',
    'Enablevalidatetrim': 'EnableValidateTrim',
    'Tooltip': 'ToolTip',
    'Tooltiptitle': 'ToolTipTitle',
    'Tooltiptype': 'ToolTipType',
    'Tooltipautohide': 'ToolTipAutoHide',
    'Requiredmessage': 'RequiredMessage',
    'Maxlengthmessage': 'MaxLengthMessage',
    'Minlengthmessage': 'MinLengthMessage',
    'Regexpattern': 'RegexPattern',
    'Regexmessage': 'RegexMessage',
    'Regexignorecase': 'RegexIgnoreCase',
    'Comparecontrol': 'CompareControl',
    'Comparevalue': 'CompareValue',
    'Compareoperator': 'CompareOperator',
    'Comparetype': 'CompareType',
    'Comparemessage': 'CompareMessage',
    'Nextfocuscontrol': 'NextFocusControl',
    'Nextclickcontrol': 'NextClickControl',
    'Enableprefix': 'EnablePrefix',
    'Enablesuffix': 'EnableSuffix',
    'Emptytext': 'EmptyText',
    'Autopostback': 'AutoPostBack',
    'Enableblurevent': 'EnableBlurEvent',
    'Controlbase': 'ControlBase',
    'Productname': 'ProductName',
    'Productversion': 'ProductVersion',
    'Attributedatatag': 'AttributeDataTag',
    'Wrapperid': 'WrapperID',
    'Id': 'ID',
    'Hidemode': 'HideMode',
    'Isfineuiajaxpostback': 'IsFineUIAjaxPostBack',
    'Isfineuipromvcajaxpostback': 'IsFineUIMvcAjaxPostBack',
    'Focusonpageload': 'FocusOnPageLoad',
    'Cssclass': 'CssClass',
    'Cssstyle': 'CssStyle',
    'Marginright': 'MarginRight',
    'Marginleft': 'MarginLeft',
    'Margintop': 'MarginTop',
    'Marginbottom': 'MarginBottom',
    'Anchorvalue': 'AnchorValue',
    'Columnwidth': 'ColumnWidth',
    'Absolutex': 'AbsoluteX',
    'Absolutey': 'AbsoluteY',
    'Tableconfigcolumns': 'TableConfigColumns',
    'Tablerowspan': 'TableRowspan',
    'Tablecolspan': 'TableColspan',
    'Boxconfigalign': 'BoxConfigAlign',
    'Boxconfigposition': 'BoxConfigPosition',
    'Boxconfigpadding': 'BoxConfigPadding',
    'Boxconfigchildmargin': 'BoxConfigChildMargin',
    'Boxflex': 'BoxFlex',
    'Regionsplit': 'RegionSplit',
    'Regionsplitwidth': 'RegionSplitWidth',
    'Regionsplitheaderclass': 'RegionSplitHeaderClass',
    'Regionposition': 'RegionPosition',
    'Enabledefaultstate': 'EnableDefaultState',
    'Enabledefaultcorner': 'EnableDefaultCorner',
    'Enablepostback': 'EnablePostBack',
    'Enablepress': 'EnablePress',
    'Onclientclick': 'OnClientClick',
    'Iconfont': 'IconFont',
    'Iconurl': 'IconUrl',
    'Iconalign': 'IconAlign',
    'Tooltip': 'ToolTip',
    'Tooltiptype': 'ToolTipType',
    'Validatetarget': 'ValidateTarget',
    'Validatemessagebox': 'ValidateMessageBox',
    'Validatemessageboxplain': 'ValidateMessageBoxPlain',
    'Showmenuicon': 'ShowMenuIcon',
    'Confirmtitle': 'ConfirmTitle',
    'Confirmtext': 'ConfirmText',
    'Confirmicon': 'ConfirmIcon',
    'Confirmtarget': 'ConfirmTarget',
    'Menuid': 'MenuID',
    'Enablepressgroup': 'EnablePressGroup',
    'Allownonepress': 'AllowNonePress',
    'Allowmultipress': 'AllowMultiPress',
    'Selectedindex': 'SelectedIndex',
    'Selectedtext': 'SelectedText',
    'Selecteditem': 'SelectedItem',
    'Selectedvalue': 'SelectedValue',
    'Enablecheckboxselect': 'EnableCheckBoxSelect',
    'Enablemultiselect': 'EnableMultiSelect',
    'Multiselectseparator': 'MultiSelectSeparator',
    'Enablegroup': 'EnableGroup',
    'Dataenableselectfield': 'DataEnableSelectField',
    'Autopostback': 'AutoPostBack',
    'Datagroupfield': 'DataGroupField',
    'Datagroupformatstring': 'DataGroupFormatString',
    'Datatextfield': 'DataTextField',
    'Datatextformatstring': 'DataTextFormatString',
    'Datavaluefield': 'DataValueField',
    'Datasource': 'DataSource',
    'Enableselect': 'EnableSelect',
    'F_items': 'F_Items',
    'Datalistitem': 'DataListItem',
    'Enableselect': 'EnableSelect',
    'Navigateurl': 'NavigateUrl',
    'Showarrow': 'ShowArrow',
    'Dataitem': 'DataItem',
    'Dataitem': 'DataItem',
    'Headerstyle': 'HeaderStyle',
    'Toolbaralign': 'ToolbarAlign',
    'Itemspace': 'ItemSpace',
    'Dateformatstring': 'DateFormatString',
    'Enabledateselectevent': 'EnableDateSelectEvent',
    'Disablecontrolbeforepostback': 'DisableControlBeforePostBack',
    'Enablepostback': 'EnablePostBack',
    'Onclientclick': 'OnClientClick',
    'Validatetarget': 'ValidateTarget',
    'Validatemessagebox': 'ValidateMessageBox',
    'Validatemessageboxplain': 'ValidateMessageBoxPlain',
    'Confirmtitle': 'ConfirmTitle',
    'Confirmtext': 'ConfirmText',
    'Confirmicon': 'ConfirmIcon',
    'Confirmtarget': 'ConfirmTarget',
    'Groupname': 'GroupName',
    'Autopostback': 'AutoPostBack',
    'Navigateurl': 'NavigateUrl',
    'Iconurl': 'IconUrl',
    'Iconfont': 'IconFont',
    'Onclientclick': 'OnClientClick',
    'Enablepostback': 'EnablePostBack',
    'Iconurl': 'IconUrl',
    'Iconfont': 'IconFont',
    'Tooltip': 'ToolTip',
    'Tooltiptype': 'ToolTipType',
    'Menuid': 'MenuID',
    'Hidemode': 'HideMode',
    'Loadingimageurl': 'LoadingImageUrl',
    'Autosizepanelid': 'AutoSizePanelID',
    'Hidescrollbars': 'HideScrollbars',
    'Enablepageloading': 'EnablePageLoading',
    'Jslibrary': 'JSLibrary',
    'Customthemebasepath': 'CustomThemeBasePath',
    'Customtheme': 'CustomTheme',
    'Customlanguagebasepath': 'CustomLanguageBasePath',
    'Customlanguage': 'CustomLanguage',
    'Enableshim': 'EnableShim',
    'Enableanimation': 'EnableAnimation',
    'Enableiframeloading': 'EnableIFrameLoading',
    'Loadingimagetype': 'LoadingImageType',
    'Ajaxloadingtext': 'AjaxLoadingText',
    'Ajaxloadingmasktext': 'AjaxLoadingMaskText',
    'Showajaxloadingmasktext': 'ShowAjaxLoadingMaskText',
    'Ajaxtimeout': 'AjaxTimeout',
    'Ieedge': 'IEEdge',
    'Mobileadaption': 'MobileAdaption',
    'Enablecompactmode': 'EnableCompactMode',
    'Enablelargemode': 'EnableLargeMode',
    'Enableformchangeconfirm': 'EnableFormChangeConfirm',
    'Simpleerror': 'SimpleError',
    'Formmessagetarget': 'FormMessageTarget',
    'Formlabelalign': 'FormLabelAlign',
    'Formredstarposition': 'FormRedStarPosition',
    'Formoffsetright': 'FormOffsetRight',
    'Formlabelwidth': 'FormLabelWidth',
    'Toolbaritemspace': 'ToolbarItemSpace',
    'Tabstriptabspace': 'TabStripTabSpace',
    'Formrowitemspace': 'FormRowItemSpace',
    'Formlabelseparator': 'FormLabelSeparator',
    'Autopostback': 'AutoPostBack',
    'Requiredmessage': 'RequiredMessage',
    'Autopostback': 'AutoPostBack',
    'Columnnumber': 'ColumnNumber',
    'Columnvertical': 'ColumnVertical',
    'Datatextfield': 'DataTextField',
    'Datatextformatstring': 'DataTextFormatString',
    'Datavaluefield': 'DataValueField',
    'Datasource': 'DataSource',
    'F_items': 'F_Items',
    'Checkitem': 'CheckItem',
    'Optionitem': 'OptionItem',
    'Persistoriginal': 'PersistOriginal',
    'Focusonpageload': 'FocusOnPageLoad',
    'Encodetext': 'EncodeText',
    'Onclientclick': 'OnClientClick',
    'Navigateurl': 'NavigateUrl',
    'Encodetext': 'EncodeText',
    'Imageurl': 'ImageUrl',
    'Imagewidth': 'ImageWidth',
    'Imageheight': 'ImageHeight',
    'Imagecssclass': 'ImageCssClass',
    'Imagecssstyle': 'ImageCssStyle',
    'Imagealt': 'ImageAlt',
    'Onclientclick': 'OnClientClick',
    'Validatetarget': 'ValidateTarget',
    'Validatemessagebox': 'ValidateMessageBox',
    'Validatemessageboxplain': 'ValidateMessageBoxPlain',
    'Enablepostback': 'EnablePostBack',
    'Encodetext': 'EncodeText',
    'Confirmtitle': 'ConfirmTitle',
    'Confirmtext': 'ConfirmText',
    'Confirmicon': 'ConfirmIcon',
    'Confirmtarget': 'ConfirmTarget',
    'Groupname': 'GroupName',
    'Autopostback': 'AutoPostBack',
    'Requiredmessage': 'RequiredMessage',
    'Autopostback': 'AutoPostBack',
    'Columnnumber': 'ColumnNumber',
    'Columnvertical': 'ColumnVertical',
    'Datatextfield': 'DataTextField',
    'Datatextformatstring': 'DataTextFormatString',
    'Datavaluefield': 'DataValueField',
    'Datasource': 'DataSource',
    'Selectedvalue': 'SelectedValue',
    'Selectedindex': 'SelectedIndex',
    'Selecteditem': 'SelectedItem',
    'F_items': 'F_Items',
    'Radioitem': 'RadioItem',
    'Enableedit': 'EnableEdit',
    'Matchfieldwidth': 'MatchFieldWidth',
    'Enablemultiselect': 'EnableMultiSelect',
    'Multiselectseparator': 'MultiSelectSeparator',
    'Poppanelid': 'PopPanelID',
    'Datacontrolid': 'DataControlID',
    'Autoshowclearicon': 'AutoShowClearIcon',
    'Enablecleariconclickevent': 'EnableClearIconClickEvent',
    'Alwaysdisplaypoppanel': 'AlwaysDisplayPopPanel',
    'Isuserinput': 'IsUserInput',
    'Emptytext': 'EmptyText',
    'Selectedindex': 'SelectedIndex',
    'Selectedtext': 'SelectedText',
    'Selecteditem': 'SelectedItem',
    'Selectedvalue': 'SelectedValue',
    'Matchfieldwidth': 'MatchFieldWidth',
    'Enablecheckboxselect': 'EnableCheckBoxSelect',
    'Autoselectfirstitem': 'AutoSelectFirstItem',
    'Enablemultiselect': 'EnableMultiSelect',
    'Multiselectseparator': 'MultiSelectSeparator',
    'Forceselection': 'ForceSelection',
    'Autoshowclearicon': 'AutoShowClearIcon',
    'Enablecleariconclickevent': 'EnableClearIconClickEvent',
    'Enableedit': 'EnableEdit',
    'Enablegroup': 'EnableGroup',
    'Enablesimulatetree': 'EnableSimulateTree',
    'Datasimulatetreelevelfield': 'DataSimulateTreeLevelField',
    'Dataenableselectfield': 'DataEnableSelectField',
    'Autopostback': 'AutoPostBack',
    'Datadisplayformatstring': 'DataDisplayFormatString',
    'Datagroupfield': 'DataGroupField',
    'Datagroupformatstring': 'DataGroupFormatString',
    'Datatextfield': 'DataTextField',
    'Datatextformatstring': 'DataTextFormatString',
    'Datavaluefield': 'DataValueField',
    'Datasource': 'DataSource',
    'F_items': 'F_Items',
    'Isuserinput': 'IsUserInput',
    'Dataitem': 'DataItem',
    'Dataitem': 'DataItem',
    'Listitem': 'ListItem',
    'Enableselect': 'EnableSelect',
    'Simulatetreelevel': 'SimulateTreeLevel',
    'Enableedit': 'EnableEdit',
    'Dateformatstring': 'DateFormatString',
    'Enabledateselectevent': 'EnableDateSelectEvent',
    'Acceptfiletypes': 'AcceptFileTypes',
    'Buttontext': 'ButtonText',
    'Buttononly': 'ButtonOnly',
    'Buttoniconfont': 'ButtonIconFont',
    'Buttonicon': 'ButtonIcon',
    'Buttoniconurl': 'ButtonIconUrl',
    'Postedfile': 'PostedFile',
    'Hasfile': 'HasFile',
    'Filename': 'FileName',
    'Shortfilename': 'ShortFileName',
    'Enablecommas': 'EnableCommas',
    'Enableround': 'EnableRound',
    'Showtrigger': 'ShowTrigger',
    'Nodecimal': 'NoDecimal',
    'Nonegative': 'NoNegative',
    'Decimalprecision': 'DecimalPrecision',
    'Nextfocuscontrol': 'NextFocusControl',
    'Nextclickcontrol': 'NextClickControl',
    'Autogrowheight': 'AutoGrowHeight',
    'Autogrowheightmax': 'AutoGrowHeightMax',
    'Autogrowheightmin': 'AutoGrowHeightMin',
    'Minheight': 'MinHeight',
    'Textmode': 'TextMode',
    'Enableedit': 'EnableEdit',
    'Timeformatstring': 'TimeFormatString',
    'Maxtimetext': 'MaxTimeText',
    'Mintimetext': 'MinTimeText',
    'Enabletimeselectevent': 'EnableTimeSelectEvent',
    'Enableedit': 'EnableEdit',
    'Showtrigger': 'ShowTrigger',
    'Enablepostback': 'EnablePostBack',
    'Triggericonurl': 'TriggerIconUrl',
    'Triggericon': 'TriggerIcon',
    'Onclienttriggerclick': 'OnClientTriggerClick',
    'Enableedit': 'EnableEdit',
    'Showtrigger': 'ShowTrigger',
    'Showtrigger1': 'ShowTrigger1',
    'Showtrigger2': 'ShowTrigger2',
    'Enabletrigger1postback': 'EnableTrigger1PostBack',
    'Enabletrigger2postback': 'EnableTrigger2PostBack',
    'Trigger1iconurl': 'Trigger1IconUrl',
    'Trigger2iconurl': 'Trigger2IconUrl',
    'Trigger1icon': 'Trigger1Icon',
    'Trigger2icon': 'Trigger2Icon',
    'Onclienttrigger1click': 'OnClientTrigger1Click',
    'Onclienttrigger2click': 'OnClientTrigger2Click',
    'Enableiframe': 'EnableIFrame',
    'Iframeurl': 'IFrameUrl',
    'Iframename': 'IFrameName',
    'Showcollapsetool': 'ShowCollapseTool',
    'Enableactiveontop': 'EnableActiveOnTop',
    'Enablefill': 'EnableFill',
    'Autopostback': 'AutoPostBack',
    'Activepaneindex': 'ActivePaneIndex',
    'Activepane': 'ActivePane',
    'Activepaneindex': 'ActivePaneIndex',
    'Enablecollapse': 'EnableCollapse',
    'Showheader': 'ShowHeader',
    'Showborder': 'ShowBorder',
    'Enablehightlight': 'EnableHightlight',
    'Enableiframe': 'EnableIFrame',
    'Iframeurl': 'IFrameUrl',
    'Iframename': 'IFrameName',
    'Formrowitemsspace': 'FormRowItemsSpace',
    'Formrowitemspace': 'FormRowItemSpace',
    'Showheader': 'ShowHeader',
    'Showborder': 'ShowBorder',
    'Enableiframe': 'EnableIFrame',
    'Iframeurl': 'IFrameUrl',
    'Iframename': 'IFrameName',
    'Enabletextselection': 'EnableTextSelection',
    'Enablenodehyperlink': 'EnableNodeHyperLink',
    'Autoscroll': 'AutoScroll',
    'Enablesingleclickexpand': 'EnableSingleClickExpand',
    'Enablesingleexpand': 'EnableSingleExpand',
    'Enableicons': 'EnableIcons',
    'Autoleafidentification': 'AutoLeafIdentification',
    'Enablemultiselect': 'EnableMultiSelect',
    'Keepcurrentselection': 'KeepCurrentSelection',
    'Hidehscrollbar': 'HideHScrollbar',
    'Expandertoright': 'ExpanderToRight',
    'Headerstyle': 'HeaderStyle',
    'Minimode': 'MiniMode',
    'Minimodepopwidth': 'MiniModePopWidth',
    'Datasource': 'DataSource',
    'Selectednode': 'SelectedNode',
    'Selectednodeid': 'SelectedNodeID',
    'F_nodes': 'F_Nodes',
    'Nodeid': 'NodeID',
    'Nodeid': 'NodeID',
    'Commandname': 'CommandName',
    'Commandargument': 'CommandArgument',
    'Nodeid': 'NodeID',
    'Xmlnode': 'XmlNode',
    'Xmlnode': 'XmlNode',
    'Treenode': 'TreeNode',
    'Treeinstance': 'TreeInstance',
    'Parentnode': 'ParentNode',
    'Enableexpandevent': 'EnableExpandEvent',
    'Enablecollapseevent': 'EnableCollapseEvent',
    'Enableclickevent': 'EnableClickEvent',
    'Onclientclick': 'OnClientClick',
    'Commandname': 'CommandName',
    'Commandargument': 'CommandArgument',
    'Enablecheckbox': 'EnableCheckBox',
    'Enablecheckevent': 'EnableCheckEvent',
    'Cssclass': 'CssClass',
    'Nodeid': 'NodeID',
    'Navigateurl': 'NavigateUrl',
    'Iconurl': 'IconUrl',
    'Iconfont': 'IconFont',
    'Tooltip': 'ToolTip',
    'Xmlattributemapping': 'XmlAttributeMapping',
    'Animationtype': 'AnimationType',
    'Enabledefaultcorner': 'EnableDefaultCorner',
    'Percentwidth': 'PercentWidth',
    'Percentheight': 'PercentHeight',
    'Keeplastposition': 'KeepLastPosition',
    'Keeplastsize': 'KeepLastSize',
    'Constraininitialsize': 'ConstrainInitialSize',
    'Windowposition': 'WindowPosition',
    'Positionx': 'PositionX',
    'Positiony': 'PositionY',
    'Fixedposition': 'FixedPosition',
    'Dependsviewportsize': 'DependsViewPortSize',
    'Enableclose': 'EnableClose',
    'Enabledrag': 'EnableDrag',
    'Enablemaximize': 'EnableMaximize',
    'Enableminimize': 'EnableMinimize',
    'Cleariframeafterclose': 'ClearIFrameAfterClose',
    'Ismodal': 'IsModal',
    'Hideonmaskclick': 'HideOnMaskClick',
    'Enableresize': 'EnableResize',
    'Onclientclosebuttonclick': 'OnClientCloseButtonClick',
    'Closeaction': 'CloseAction',
    'Closeargument': 'CloseArgument',
    'Enableiframe': 'EnableIFrame',
    'Iframeurl': 'IFrameUrl',
    'Iframename': 'IFrameName',
    'Allowcellediting': 'AllowCellEditing',
    'Clickstoedit': 'ClicksToEdit',
    'Allowcolumnlocking': 'AllowColumnLocking',
    'Allowfilters': 'AllowFilters',
    'Filtereddata': 'FilteredData',
    'Autoselecteditor': 'AutoSelectEditor',
    'Allowpaging': 'AllowPaging',
    'Isdatabasepaging': 'IsDatabasePaging',
    'Clearselectedrowsafterpaging': 'ClearSelectedRowsAfterPaging',
    'Pagesize': 'PageSize',
    'Pageindex': 'PageIndex',
    'Pagecount': 'PageCount',
    'Recordcount': 'RecordCount',
    'Allowsorting': 'AllowSorting',
    'Isdatabasesorting': 'IsDatabaseSorting',
    'Sortfield': 'SortField',
    'Enablesummary': 'EnableSummary',
    'Summarydata': 'SummaryData',
    'Summaryposition': 'SummaryPosition',
    'Dataurl': 'DataUrl',
    'Datamethod': 'DataMethod',
    'Rowrendererfunction': 'RowRendererFunction',
    'Rowdataboundfunction': 'RowDataBoundFunction',
    'Enablebigdata': 'EnableBigData',
    'Enablebigdatarowtip': 'EnableBigDataRowTip',
    'Mincolumnwidth': 'MinColumnWidth',
    'Disableunselectablerows': 'DisableUnselectableRows',
    'Tabverticalnavigate': 'TabVerticalNavigate',
    'Tabeditablecell': 'TabEditableCell',
    'Entersameastab': 'EnterSameAsTab',
    'Dataidfield': 'DataIDField',
    'Datatextfield': 'DataTextField',
    'Emptytext': 'EmptyText',
    'Rowverticalalign': 'RowVerticalAlign',
    'Expandallrowexpanders': 'ExpandAllRowExpanders',
    'Enabletextselection': 'EnableTextSelection',
    'Showgridheader': 'ShowGridHeader',
    'Enableheadermenu': 'EnableHeaderMenu',
    'Enablecolumnlines': 'EnableColumnLines',
    'Enablecolumnresize': 'EnableColumnResize',
    'Enablecolumnmove': 'EnableColumnMove',
    'Enablesamegroupcolumnmove': 'EnableSameGroupColumnMove',
    'Enablerowlines': 'EnableRowLines',
    'Enablealternaterowcolor': 'EnableAlternateRowColor',
    'Enablerowselectevent': 'EnableRowSelectEvent',
    'Enablerowdeselectevent': 'EnableRowDeselectEvent',
    'Enablerowclickevent': 'EnableRowClickEvent',
    'Enablerowdoubleclickevent': 'EnableRowDoubleClickEvent',
    'Enableaftereditevent': 'EnableAfterEditEvent',
    'Autoexpandcolumn': 'AutoExpandColumn',
    'Forcefit': 'ForceFit',
    'Enablecheckboxselect': 'EnableCheckBoxSelect',
    'Checkboxselectonly': 'CheckBoxSelectOnly',
    'Enablemultiselect': 'EnableMultiSelect',
    'Keepcurrentselection': 'KeepCurrentSelection',
    'Showpagingmessage': 'ShowPagingMessage',
    'Selectedrowindex': 'SelectedRowIndex',
    'Selectedrow': 'SelectedRow',
    'Selectedrowid': 'SelectedRowID',
    'Datasource': 'DataSource',
    'Pageitems': 'PageItems',
    'Enabletree': 'EnableTree',
    'Expandalltreenodes': 'ExpandAllTreeNodes',
    'Dataparentidfield': 'DataParentIDField',
    'Treecolumn': 'TreeColumn',
    'Enabletreeicons': 'EnableTreeIcons',
    'F_rows': 'F_Rows',
    'Rowindex': 'RowIndex',
    'Columnindex': 'ColumnIndex',
    'Columnid': 'ColumnID',
    'Rowid': 'RowID',
    'Rowindex': 'RowIndex',
    'Rowid': 'RowID',
    'Columnindex': 'ColumnIndex',
    'Columnid': 'ColumnID',
    'Commandname': 'CommandName',
    'Commandargument': 'CommandArgument',
    'Newpageindex': 'NewPageIndex',
    'Dataitem': 'DataItem',
    'Rowindex': 'RowIndex',
    'Rowindex': 'RowIndex',
    'Rowid': 'RowID',
    'Rowcssclass': 'RowCssClass',
    'Rowattributes': 'RowAttributes',
    'Rowselectable': 'RowSelectable',
    'Dataitem': 'DataItem',
    'Rowindex': 'RowIndex',
    'Rowid': 'RowID',
    'Rowtext': 'RowText',
    'Rowparentid': 'RowParentID',
    'Treenodeiconurl': 'TreeNodeIconUrl',
    'Treenodeicon': 'TreeNodeIcon',
    'Treenodeiconfont': 'TreeNodeIconFont',
    'Rowcssclass': 'RowCssClass',
    'Rowselectable': 'RowSelectable',
    'Dataitem': 'DataItem',
    'Rowindex': 'RowIndex',
    'Rowindex': 'RowIndex',
    'Rowid': 'RowID',
    'Sortfield': 'SortField',
    'Sortdirection': 'SortDirection',
    'Columnindex': 'ColumnIndex',
    'Columnid': 'ColumnID',
    'Datasimulatetreelevelfield': 'DataSimulateTreeLevelField',
    'Tooltip': 'ToolTip',
    'Tooltiptype': 'ToolTipType',
    'Datatooltipfield': 'DataToolTipField',
    'Datatooltipformatstring': 'DataToolTipFormatString',
    'Datafield': 'DataField',
    'Dataformatstring': 'DataFormatString',
    'Nulldisplaytext': 'NullDisplayText',
    'Htmlencode': 'HtmlEncode',
    'Htmlencodeformatstring': 'HtmlEncodeFormatString',
    'Autopostback': 'AutoPostBack',
    'Datafield': 'DataField',
    'Renderasstaticfield': 'RenderAsStaticField',
    'Showheadercheckbox': 'ShowHeaderCheckBox',
    'Datacommandnamefield': 'DataCommandNameField',
    'Datacommandargumentfield': 'DataCommandArgumentField',
    'Commandname': 'CommandName',
    'Commandargument': 'CommandArgument',
    'Datatextfield': 'DataTextField',
    'Datatextformatstring': 'DataTextFormatString',
    'Urlencode': 'UrlEncode',
    'Datanavigateurlformatstring': 'DataNavigateUrlFormatString',
    'Navigateurl': 'NavigateUrl',
    'Htmlencode': 'HtmlEncode',
    'Htmlencodeformatstring': 'HtmlEncodeFormatString',
    'Dataimageurlfield': 'DataImageUrlField',
    'Dataimageurlformatstring': 'DataImageUrlFormatString',
    'Imagewidth': 'ImageWidth',
    'Imageheight': 'ImageHeight',
    'Imageurl': 'ImageUrl',
    'Enableheadermenu': 'EnableHeaderMenu',
    'Enablecolumnhide': 'EnableColumnHide',
    'Datatextfield': 'DataTextField',
    'Datatextformatstring': 'DataTextFormatString',
    'Htmlencode': 'HtmlEncode',
    'Htmlencodeformatstring': 'HtmlEncodeFormatString',
    'Enablepostback': 'EnablePostBack',
    'Onclientclick': 'OnClientClick',
    'Validatetarget': 'ValidateTarget',
    'Validatemessagebox': 'ValidateMessageBox',
    'Validatemessageboxplain': 'ValidateMessageBoxPlain',
    'Iconfont': 'IconFont',
    'Iconurl': 'IconUrl',
    'Confirmtitle': 'ConfirmTitle',
    'Confirmtext': 'ConfirmText',
    'Confirmicon': 'ConfirmIcon',
    'Confirmtarget': 'ConfirmTarget',
    'Datacommandnamefield': 'DataCommandNameField',
    'Datacommandargumentfield': 'DataCommandArgumentField',
    'Commandname': 'CommandName',
    'Commandargument': 'CommandArgument',
    'Enableresize': 'EnableResize',
    'Enableheadermenu': 'EnableHeaderMenu',
    'Enablecolumnhide': 'EnableColumnHide',
    'Enablepagingnumber': 'EnablePagingNumber',
    'Enabletreenumber': 'EnableTreeNumber',
    'Enablelock': 'EnableLock',
    'Itemtemplate': 'ItemTemplate',
    'Renderasrowexpander': 'RenderAsRowExpander',
    'Expandondoubleclick': 'ExpandOnDoubleClick',
    'Expandonenter': 'ExpandOnEnter',
    'Expandtoselectrow': 'ExpandToSelectRow',
    'Enableheadermenu': 'EnableHeaderMenu',
    'Enablecolumnhide': 'EnableColumnHide',
    'Windowid': 'WindowID',
    'Datawindowtitlefield': 'DataWindowTitleField',
    'Datawindowtitleformatstring': 'DataWindowTitleFormatString',
    'Datatextfield': 'DataTextField',
    'Datatextformatstring': 'DataTextFormatString',
    'Htmlencode': 'HtmlEncode',
    'Htmlencodeformatstring': 'HtmlEncodeFormatString',
    'Dataiframeurlfields': 'DataIFrameUrlFields',
    'Dataiframeurlformatstring': 'DataIFrameUrlFormatString',
    'Urlencode': 'UrlEncode',
    'Iframeurl': 'IFrameUrl',
    'Iconfont': 'IconFont',
    'Iconurl': 'IconUrl',
    'Columnindex': 'ColumnIndex',
    'Enablefilter': 'EnableFilter',
    'Enableresize': 'EnableResize',
    'Sortfield': 'SortField',
    'Enablelock': 'EnableLock',
    'Columnid': 'ColumnID',
    'Headertext': 'HeaderText',
    'Headertooltip': 'HeaderToolTip',
    'Headertooltiptype': 'HeaderToolTipType',
    'Minwidth': 'MinWidth',
    'Boxflex': 'BoxFlex',
    'Expandunusedspace': 'ExpandUnusedSpace',
    'Textalign': 'TextAlign',
    'Headertextalign': 'HeaderTextAlign',
    'Enableheadermenu': 'EnableHeaderMenu',
    'Enablecolumnhide': 'EnableColumnHide',
    'Gridfilter': 'GridFilter',
    'Enablemultifilter': 'EnableMultiFilter',
    'Showmatcher': 'ShowMatcher',
    'Matcherdefault': 'MatcherDefault',
    'Fieldrequired': 'FieldRequired',
    'Datafield': 'DataField',
    'Enablecolumnedit': 'EnableColumnEdit',
    'Renderasstaticfield': 'RenderAsStaticField',
    'Enablecolumnedit': 'EnableColumnEdit',
    'Fieldtype': 'FieldType',
    'Rendererargument': 'RendererArgument',
    'Rendererfunction': 'RendererFunction',
    'Nulldisplaytext': 'NullDisplayText',
    'Htmlencode': 'HtmlEncode',
    'Autoselecteditor': 'AutoSelectEditor',
    'Enableiframe': 'EnableIFrame',
    'Iframeurl': 'IFrameUrl',
    'Iframename': 'IFrameName',
    'Enableiframe': 'EnableIFrame',
    'Iframeurl': 'IFrameUrl',
    'Iframename': 'IFrameName',
    'Showtabheader': 'ShowTabHeader',
    'Animationtype': 'AnimationType',
    'Closeondblclick': 'CloseOnDblclick',
    'Autopostback': 'AutoPostBack',
    'Enabletabclosemenu': 'EnableTabCloseMenu',
    'Tabposition': 'TabPosition',
    'Tabalign': 'TabAlign',
    'Tabfill': 'TabFill',
    'Tabspace': 'TabSpace',
    'Tabbordercolor': 'TabBorderColor',
    'Tabplain': 'TabPlain',
    'Activetabindex': 'ActiveTabIndex',
    'Activetab': 'ActiveTab',
    'Showheader': 'ShowHeader',
    'Showborder': 'ShowBorder',
    'Enablecollapse': 'EnableCollapse',
    'Enableclose': 'EnableClose',
    'Ondateselect': 'OnDateSelect',
    'Textbox': 'TextBox',
    'Validateforms': 'ValidateForms',
    'Simpleform': 'SimpleForm',
    'Onblur': 'OnBlur',
    'Ontextchanged': 'OnTextChanged',
    'Textarea': 'TextArea',
    'Htmleditor': 'HtmlEditor',
    'Basepath': 'BasePath',
    'Triggerbox': 'TriggerBox',
    'Twintriggerbox': 'TwinTriggerBox',
    'Ontriggerclick': 'OnTriggerClick',
    'Ontrigger1click': 'OnTrigger1Click',
    'Ontrigger2click': 'OnTrigger2Click',
    'Minvalue': 'MinValue',
    'Maxvalue': 'MaxValue',
    'Numberbox': 'NumberBox',
    'Datepicker': 'DatePicker'

};