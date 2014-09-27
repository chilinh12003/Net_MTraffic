<%@ Control Inherits="CuteEditor.EditorUtilityCtrl" Language="c#" AutoEventWireup="false" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<fieldset>
	<legend>
	[[Cursor]]
	</legend>
	<select id="sel_cursor">
		<option value="">[[NotSet]]</option>
		<option value="Default">[[Default]]</option>
		<option value="Move">[[Move]]</option>
		<option value="Text">Text</option>
		<option value="Wait">Wait</option>
		<option value="Help">Help</option>
		<!-- x-resize -->
	</select>
</fieldset>

<fieldset>
	<legend>
	[[Filter]]
	</legend>
	<input type="text" id="inp_filter" style="width:240px" /> <!--button filter builder-->
</fieldset>
<div id="outer" style="height:100px; margin-bottom:10px; padding:5px;"><div id="div_demo">[[DemoText]]</div></div><br />

<script type="text/javascript">

var OxO2287=["sel_cursor","inp_filter","outer","div_demo","cssText","style","value","cursor","filter"];var sel_cursor=Window_GetElement(window,OxO2287[0],true);var inp_filter=Window_GetElement(window,OxO2287[1],true);var outer=Window_GetElement(window,OxO2287[2],true);var div_demo=Window_GetElement(window,OxO2287[3],true);function UpdateState(){div_demo[OxO2287[5]][OxO2287[4]]=element[OxO2287[5]][OxO2287[4]];} ;function SyncToView(){sel_cursor[OxO2287[6]]=element[OxO2287[5]][OxO2287[7]];if(Browser_IsWinIE()){inp_filter[OxO2287[6]]=element[OxO2287[5]][OxO2287[8]];} ;} ;function SyncTo(element){element[OxO2287[5]][OxO2287[7]]=sel_cursor[OxO2287[6]];if(Browser_IsWinIE()){element[OxO2287[5]][OxO2287[8]]=inp_filter[OxO2287[6]];} ;} ;

</script>
