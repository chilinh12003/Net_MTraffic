<%@ Page Language="C#" Inherits="CuteEditor.Dialogs.ThumbnailPage" %>

<script runat="server">
override protected void OnInit(EventArgs args)
{
	if(Context.Request.QueryString["Dialog"]=="Standard")
	{
		if(Context.Request.QueryString["IsFrame"]==null)
		{
			string FrameSrc="Thumbnail.aspx?IsFrame=1&"+Request.ServerVariables["QUERY_STRING"];
			CuteEditor.CEU.WriteDialogOuterFrame(Context,"[[AutoThumbnail]]",FrameSrc);
			Context.Response.End();
		}
	}
		base.OnInit(args);
}
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>[[AutoThumbnail]] </title>		
		<meta http-equiv="Page-Enter" content="blendTrans(Duration=0.1)" />
		<meta http-equiv="Page-Exit" content="blendTrans(Duration=0.1)" />
		<link href='Load.ashx?type=themecss&file=dialog.css&theme=[[_Theme_]]' type="text/css" rel="stylesheet" />
		<script type="text/javascript" src="Load.ashx?type=dialogscript&file=DialogHead.js"></script>
	</head>
	<body style="margin:0px;border-width:0px;padding:4px;">
		<form runat="server" id="Form1">
			<input type="hidden" runat="server" id="hiddenDirectory" name="hiddenDirectory" /> 
			<input type="hidden" runat="server" id="hiddenFile" name="hiddenFile" />
			<input type="hidden" runat="server" enableviewstate="false" id="hiddenAlert" name="hiddenAlert" /> 
			<input type="hidden" runat="server" enableviewstate="false" id="hiddenAction" name="hiddenAction" />
		<table border="0" cellpadding="4" cellspacing="0" width="100%">
			<tr>
				<td>
					<table border="0" cellpadding="1" cellspacing="4" class="normal">
					<tr>
						<td style="white-space:nowrap"  valign="top">
							<fieldset style="height:80px;">
								<table border="0" cellpadding="1" cellspacing="0" class="normal">
									<tr>
										<td style="white-space:nowrap; width:60" >[[Width]]:</td>
										<td>
											<input runat="server" id="inp_width" value="80" maxlength="3" onkeyup="checkConstrains('width');"  onkeypress="return CancelEventIfNotDigit()" style="WIDTH : 70px" name="inp_width" />
										</td>
										<td rowspan="2" align="right" valign="middle"><img src="Load.ashx?type=image&file=locked.gif" id="imgLock" width="25" height="32" title="[[ConstrainProportions]]" /></td>
									</tr>
									<tr>
										<td>[[Height]]:</td>
										<td>
											<input runat="server" id="inp_height" value="80" maxlength="3" onkeyup="checkConstrains('height');"  onkeypress="return CancelEventIfNotDigit()" style="WIDTH : 70px" name="inp_height" />
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<input type="checkbox" id="constrain_prop" checked="checked" onclick="javascript:toggleConstrains();" />
											 [[ConstrainProportions]]
										</td>
									</tr>
								</table>
							</fieldset>		
						</td>
						<td valign="top" style="white-space:nowrap" >
							<div style="width:100px; height:80px; vertical-align:top;overflow:hidden;BACKGROUND-COLOR: #ffffff;BORDER-RIGHT: buttonhighlight 1px solid;BORDER-TOP: buttonshadow 1px solid;BORDER-LEFT: buttonshadow 1px solid;BORDER-BOTTOM: buttonhighlight 1px solid;">
								<img alt="" id="img_demo" src="Load.ashx?type=image&file=1x1.gif" />
							</div>
						</td>
					</tr>	
					<tr>
						<td>
							<div style="margin-top:8px;text-align:center">
								<asp:Button id="okButton" Text="  [[OK]]  " CssClass="formbutton" Runat="server" OnClick="thumbnailButton_Click" />
								&nbsp;&nbsp;
								<input type="button" value="[[Cancel]]" class="formbutton" onclick="top.returnValue=false;top.close()" />
							</div>
						</td>
					</tr>				
				</table>
			</td>
		</tr>
		</table>
	</form>			
	</body>
</html>
	
		<script type="text/javascript">
			var OxOebd1=["src","img_demo","inp_width","inp_height","hiddenFile","hiddenAlert","hiddenDirectory","hiddenAction","onload","value","","IMG","width","height","[[ImagetooSmall]]","dir","imgLock","constrain_prop","checked","Load.ashx?type=image\x26file=locked.gif","Load.ashx?type=image\x26file=1x1.gif","preview_image","DIV","cssText","style","position:absolute","body","offsetWidth","offsetHeight","length"];var obj=Window_GetDialogArguments(window);var src=obj[OxOebd1[0]];var img_demo=document.getElementById(OxOebd1[1]);var inp_width=document.getElementById(OxOebd1[2]);var inp_height=document.getElementById(OxOebd1[3]);var hiddenFile=Window_GetElement(window,OxOebd1[4],true);var hiddenAlert=Window_GetElement(window,OxOebd1[5],true);var hiddenDirectory=Window_GetElement(window,OxOebd1[6],true);var hiddenAction=Window_GetElement(window,OxOebd1[7],true);var defaultwidth=<%= secset.ThumbnailWidth %>;var defaultheight=<%= secset.ThumbnailHeight %>;window[OxOebd1[8]]=reset_hiddens;function reset_hiddens(){if(hiddenAction[OxOebd1[9]]!=OxOebd1[10]){if(hiddenAlert[OxOebd1[9]]){alert(hiddenAlert.value);} ;Window_SetDialogReturnValue(window,hiddenAction.value);Window_CloseDialog(window);} else {hiddenAlert[OxOebd1[9]]=OxOebd1[10];hiddenAction[OxOebd1[9]]=OxOebd1[10];} ;} ;SyncToView();function SyncToView(){if(src){var img=document.createElement(OxOebd1[11]);img[OxOebd1[0]]=src;img_demo[OxOebd1[0]]=src;var p1=parseInt(img[OxOebd1[12]]/defaultwidth);var Ox6b=parseInt(img[OxOebd1[13]]/defaultheight);if(p1>Ox6b){if(img[OxOebd1[12]]<defaultwidth){alert(OxOebd1[14]);inp_width[OxOebd1[9]]=img[OxOebd1[12]];inp_height[OxOebd1[9]]=img[OxOebd1[13]];} else {inp_width[OxOebd1[9]]=defaultwidth;var Ox6c=parseInt(defaultwidth*img[OxOebd1[13]]/img[OxOebd1[12]]);inp_height[OxOebd1[9]]=Ox6c;} ;} else {if(img[OxOebd1[13]]<defaultheight){alert(OxOebd1[14]);inp_width[OxOebd1[9]]=img[OxOebd1[12]];inp_height[OxOebd1[9]]=img[OxOebd1[13]];} else {inp_height[OxOebd1[9]]=defaultheight;var Ox6d=parseInt(defaultwidth*img[OxOebd1[12]]/img[OxOebd1[13]]);inp_width[OxOebd1[9]]=Ox6d;} ;} ;hiddenFile[OxOebd1[9]]=src;hiddenDirectory[OxOebd1[9]]=obj[OxOebd1[15]];do_preview();} ;} ;function toggleConstrains(){var Ox6f=document.getElementById(OxOebd1[16]);var Ox70=document.getElementById(OxOebd1[17]);if(Ox70[OxOebd1[18]]){Ox6f[OxOebd1[0]]=OxOebd1[19];checkConstrains(OxOebd1[12]);} else {Ox6f[OxOebd1[0]]=OxOebd1[20];} ;} ;var checkingConstrains=false;function checkConstrains(Ox73){if(checkingConstrains){return ;} ;checkingConstrains=true;try{var Ox70=document.getElementById(OxOebd1[17]);if(Ox70[OxOebd1[18]]){var Ox74=document.getElementById(OxOebd1[21]);if(Ox74){var Ox75=document.createElement(OxOebd1[22]);Ox75[OxOebd1[24]][OxOebd1[23]]=OxOebd1[25];document[OxOebd1[26]].appendChild(Ox75);Ox75.appendChild(Ox74);Ox74.removeAttribute(OxOebd1[12]);Ox74.removeAttribute(OxOebd1[13]);Ox74[OxOebd1[24]][OxOebd1[12]]=OxOebd1[10];Ox74[OxOebd1[24]][OxOebd1[13]]=OxOebd1[10];original_width=Ox74[OxOebd1[27]];original_height=Ox74[OxOebd1[28]];Ox75.removeNode(true);} else {var Ox76=document.createElement(OxOebd1[11]);Ox76[OxOebd1[0]]=src;original_width=Ox76[OxOebd1[12]];original_height=Ox76[OxOebd1[13]];} ;if((original_width>0)&&(original_height>0)){width=inp_width[OxOebd1[9]];height=inp_height[OxOebd1[9]];if(Ox73==OxOebd1[12]){if(width[OxOebd1[29]]==0||isNaN(width)){inp_width[OxOebd1[9]]=OxOebd1[10];inp_height[OxOebd1[9]]=OxOebd1[10];} else {height=parseInt(width*original_height/original_width);inp_height[OxOebd1[9]]=height;} ;} ;if(Ox73==OxOebd1[13]){if(height[OxOebd1[29]]==0||isNaN(height)){inp_width[OxOebd1[9]]=OxOebd1[10];inp_height[OxOebd1[9]]=OxOebd1[10];} else {width=parseInt(height*original_width/original_height);inp_width[OxOebd1[9]]=width;} ;} ;} ;} ;do_preview();} finally{checkingConstrains=false;} ;} ;function do_preview(){img_demo[OxOebd1[12]]=inp_width[OxOebd1[9]];img_demo[OxOebd1[13]]=inp_height[OxOebd1[9]];} ;	
			
		</script>

