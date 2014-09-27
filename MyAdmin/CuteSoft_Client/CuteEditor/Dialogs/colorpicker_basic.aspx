<%@ Page Language="C#" Inherits="CuteEditor.EditorUtilityPage" %>
<script runat="server">
string GetDialogQueryString;
override protected void OnInit(EventArgs args)
{
	if(Context.Request.QueryString["Dialog"]=="Standard")
	{	
	if(Context.Request.QueryString["IsFrame"]==null)
	{
		string FrameSrc="colorpicker_basic.aspx?IsFrame=1&"+Request.ServerVariables["QUERY_STRING"];
		CuteEditor.CEU.WriteDialogOuterFrame(Context,"[[MoreColors]]",FrameSrc);
		Context.Response.End();
	}
	}
	string s="";
	if(Context.Request.QueryString["Dialog"]=="Standard")	
		s="&Dialog=Standard";
	
	GetDialogQueryString="Theme="+Context.Request.QueryString["Theme"]+s;	
	base.OnInit(args);
}
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<meta http-equiv="Page-Enter" content="blendTrans(Duration=0.1)" />
		<meta http-equiv="Page-Exit" content="blendTrans(Duration=0.1)" />
		<script type="text/javascript" src="Load.ashx?type=dialogscript&file=DialogHead.js"></script>
		<script type="text/javascript" src="Load.ashx?type=dialogscript&file=Dialog_ColorPicker.js"></script>
		<link href='Load.ashx?type=themecss&file=dialog.css&theme=[[_Theme_]]' type="text/css"
			rel="stylesheet" />
		<style type="text/css">
			.colorcell
			{
				width:16px;
				height:17px;
				cursor:hand;
			}
			.colordiv,.customdiv
			{
				border:solid 1px #808080;
				width:16px;
				height:17px;
				font-size:1px;
			}
		</style>
		<title>[[NamedColors]]</title>
		<script>
								
		var OxOf0b3=["Green","#008000","Lime","#00FF00","Teal","#008080","Aqua","#00FFFF","Navy","#000080","Blue","#0000FF","Purple","#800080","Fuchsia","#FF00FF","Maroon","#800000","Red","#FF0000","Olive","#808000","Yellow","#FFFF00","White","#FFFFFF","Silver","#C0C0C0","Gray","#808080","Black","#000000","DarkOliveGreen","#556B2F","DarkGreen","#006400","DarkSlateGray","#2F4F4F","SlateGray","#708090","DarkBlue","#00008B","MidnightBlue","#191970","Indigo","#4B0082","DarkMagenta","#8B008B","Brown","#A52A2A","DarkRed","#8B0000","Sienna","#A0522D","SaddleBrown","#8B4513","DarkGoldenrod","#B8860B","Beige","#F5F5DC","HoneyDew","#F0FFF0","DimGray","#696969","OliveDrab","#6B8E23","ForestGreen","#228B22","DarkCyan","#008B8B","LightSlateGray","#778899","MediumBlue","#0000CD","DarkSlateBlue","#483D8B","DarkViolet","#9400D3","MediumVioletRed","#C71585","IndianRed","#CD5C5C","Firebrick","#B22222","Chocolate","#D2691E","Peru","#CD853F","Goldenrod","#DAA520","LightGoldenrodYellow","#FAFAD2","MintCream","#F5FFFA","DarkGray","#A9A9A9","YellowGreen","#9ACD32","SeaGreen","#2E8B57","CadetBlue","#5F9EA0","SteelBlue","#4682B4","RoyalBlue","#4169E1","BlueViolet","#8A2BE2","DarkOrchid","#9932CC","DeepPink","#FF1493","RosyBrown","#BC8F8F","Crimson","#DC143C","DarkOrange","#FF8C00","BurlyWood","#DEB887","DarkKhaki","#BDB76B","LightYellow","#FFFFE0","Azure","#F0FFFF","LightGray","#D3D3D3","LawnGreen","#7CFC00","MediumSeaGreen","#3CB371","LightSeaGreen","#20B2AA","DeepSkyBlue","#00BFFF","DodgerBlue","#1E90FF","SlateBlue","#6A5ACD","MediumOrchid","#BA55D3","PaleVioletRed","#DB7093","Salmon","#FA8072","OrangeRed","#FF4500","SandyBrown","#F4A460","Tan","#D2B48C","Gold","#FFD700","Ivory","#FFFFF0","GhostWhite","#F8F8FF","Gainsboro","#DCDCDC","Chartreuse","#7FFF00","LimeGreen","#32CD32","MediumAquamarine","#66CDAA","DarkTurquoise","#00CED1","CornflowerBlue","#6495ED","MediumSlateBlue","#7B68EE","Orchid","#DA70D6","HotPink","#FF69B4","LightCoral","#F08080","Tomato","#FF6347","Orange","#FFA500","Bisque","#FFE4C4","Khaki","#F0E68C","Cornsilk","#FFF8DC","Linen","#FAF0E6","WhiteSmoke","#F5F5F5","GreenYellow","#ADFF2F","DarkSeaGreen","#8FBC8B","Turquoise","#40E0D0","MediumTurquoise","#48D1CC","SkyBlue","#87CEEB","MediumPurple","#9370DB","Violet","#EE82EE","LightPink","#FFB6C1","DarkSalmon","#E9967A","Coral","#FF7F50","NavajoWhite","#FFDEAD","BlanchedAlmond","#FFEBCD","PaleGoldenrod","#EEE8AA","Oldlace","#FDF5E6","Seashell","#FFF5EE","PaleGreen","#98FB98","SpringGreen","#00FF7F","Aquamarine","#7FFFD4","PowderBlue","#B0E0E6","LightSkyBlue","#87CEFA","LightSteelBlue","#B0C4DE","Plum","#DDA0DD","Pink","#FFC0CB","LightSalmon","#FFA07A","Wheat","#F5DEB3","Moccasin","#FFE4B5","AntiqueWhite","#FAEBD7","LemonChiffon","#FFFACD","FloralWhite","#FFFAF0","Snow","#FFFAFA","AliceBlue","#F0F8FF","LightGreen","#90EE90","MediumSpringGreen","#00FA9A","PaleTurquoise","#AFEEEE","LightCyan","#E0FFFF","LightBlue","#ADD8E6","Lavender","#E6E6FA","Thistle","#D8BFD8","MistyRose","#FFE4E1","Peachpuff","#FFDAB9","PapayaWhip","#FFEFD5"];var colorlist=[{n:OxOf0b3[0],h:OxOf0b3[1]},{n:OxOf0b3[2],h:OxOf0b3[3]},{n:OxOf0b3[4],h:OxOf0b3[5]},{n:OxOf0b3[6],h:OxOf0b3[7]},{n:OxOf0b3[8],h:OxOf0b3[9]},{n:OxOf0b3[10],h:OxOf0b3[11]},{n:OxOf0b3[12],h:OxOf0b3[13]},{n:OxOf0b3[14],h:OxOf0b3[15]},{n:OxOf0b3[16],h:OxOf0b3[17]},{n:OxOf0b3[18],h:OxOf0b3[19]},{n:OxOf0b3[20],h:OxOf0b3[21]},{n:OxOf0b3[22],h:OxOf0b3[23]},{n:OxOf0b3[24],h:OxOf0b3[25]},{n:OxOf0b3[26],h:OxOf0b3[27]},{n:OxOf0b3[28],h:OxOf0b3[29]},{n:OxOf0b3[30],h:OxOf0b3[31]}];var colormore=[{n:OxOf0b3[32],h:OxOf0b3[33]},{n:OxOf0b3[34],h:OxOf0b3[35]},{n:OxOf0b3[36],h:OxOf0b3[37]},{n:OxOf0b3[38],h:OxOf0b3[39]},{n:OxOf0b3[40],h:OxOf0b3[41]},{n:OxOf0b3[42],h:OxOf0b3[43]},{n:OxOf0b3[44],h:OxOf0b3[45]},{n:OxOf0b3[46],h:OxOf0b3[47]},{n:OxOf0b3[48],h:OxOf0b3[49]},{n:OxOf0b3[50],h:OxOf0b3[51]},{n:OxOf0b3[52],h:OxOf0b3[53]},{n:OxOf0b3[54],h:OxOf0b3[55]},{n:OxOf0b3[56],h:OxOf0b3[57]},{n:OxOf0b3[58],h:OxOf0b3[59]},{n:OxOf0b3[60],h:OxOf0b3[61]},{n:OxOf0b3[62],h:OxOf0b3[63]},{n:OxOf0b3[64],h:OxOf0b3[65]},{n:OxOf0b3[66],h:OxOf0b3[67]},{n:OxOf0b3[68],h:OxOf0b3[69]},{n:OxOf0b3[70],h:OxOf0b3[71]},{n:OxOf0b3[72],h:OxOf0b3[73]},{n:OxOf0b3[74],h:OxOf0b3[75]},{n:OxOf0b3[76],h:OxOf0b3[77]},{n:OxOf0b3[78],h:OxOf0b3[79]},{n:OxOf0b3[80],h:OxOf0b3[81]},{n:OxOf0b3[82],h:OxOf0b3[83]},{n:OxOf0b3[84],h:OxOf0b3[85]},{n:OxOf0b3[86],h:OxOf0b3[87]},{n:OxOf0b3[88],h:OxOf0b3[89]},{n:OxOf0b3[90],h:OxOf0b3[91]},{n:OxOf0b3[92],h:OxOf0b3[93]},{n:OxOf0b3[94],h:OxOf0b3[95]},{n:OxOf0b3[96],h:OxOf0b3[97]},{n:OxOf0b3[98],h:OxOf0b3[99]},{n:OxOf0b3[100],h:OxOf0b3[101]},{n:OxOf0b3[102],h:OxOf0b3[103]},{n:OxOf0b3[104],h:OxOf0b3[105]},{n:OxOf0b3[106],h:OxOf0b3[107]},{n:OxOf0b3[108],h:OxOf0b3[109]},{n:OxOf0b3[110],h:OxOf0b3[111]},{n:OxOf0b3[112],h:OxOf0b3[113]},{n:OxOf0b3[114],h:OxOf0b3[115]},{n:OxOf0b3[116],h:OxOf0b3[117]},{n:OxOf0b3[118],h:OxOf0b3[119]},{n:OxOf0b3[120],h:OxOf0b3[121]},{n:OxOf0b3[122],h:OxOf0b3[123]},{n:OxOf0b3[124],h:OxOf0b3[125]},{n:OxOf0b3[126],h:OxOf0b3[127]},{n:OxOf0b3[128],h:OxOf0b3[129]},{n:OxOf0b3[130],h:OxOf0b3[131]},{n:OxOf0b3[132],h:OxOf0b3[133]},{n:OxOf0b3[134],h:OxOf0b3[135]},{n:OxOf0b3[136],h:OxOf0b3[137]},{n:OxOf0b3[138],h:OxOf0b3[139]},{n:OxOf0b3[140],h:OxOf0b3[141]},{n:OxOf0b3[142],h:OxOf0b3[143]},{n:OxOf0b3[144],h:OxOf0b3[145]},{n:OxOf0b3[146],h:OxOf0b3[147]},{n:OxOf0b3[148],h:OxOf0b3[149]},{n:OxOf0b3[150],h:OxOf0b3[151]},{n:OxOf0b3[152],h:OxOf0b3[153]},{n:OxOf0b3[154],h:OxOf0b3[155]},{n:OxOf0b3[156],h:OxOf0b3[157]},{n:OxOf0b3[158],h:OxOf0b3[159]},{n:OxOf0b3[160],h:OxOf0b3[161]},{n:OxOf0b3[162],h:OxOf0b3[163]},{n:OxOf0b3[164],h:OxOf0b3[165]},{n:OxOf0b3[166],h:OxOf0b3[167]},{n:OxOf0b3[168],h:OxOf0b3[169]},{n:OxOf0b3[170],h:OxOf0b3[171]},{n:OxOf0b3[172],h:OxOf0b3[173]},{n:OxOf0b3[174],h:OxOf0b3[175]},{n:OxOf0b3[176],h:OxOf0b3[177]},{n:OxOf0b3[178],h:OxOf0b3[179]},{n:OxOf0b3[180],h:OxOf0b3[181]},{n:OxOf0b3[182],h:OxOf0b3[183]},{n:OxOf0b3[184],h:OxOf0b3[185]},{n:OxOf0b3[186],h:OxOf0b3[187]},{n:OxOf0b3[188],h:OxOf0b3[189]},{n:OxOf0b3[190],h:OxOf0b3[191]},{n:OxOf0b3[192],h:OxOf0b3[193]},{n:OxOf0b3[194],h:OxOf0b3[195]},{n:OxOf0b3[196],h:OxOf0b3[197]},{n:OxOf0b3[198],h:OxOf0b3[199]},{n:OxOf0b3[200],h:OxOf0b3[201]},{n:OxOf0b3[202],h:OxOf0b3[203]},{n:OxOf0b3[204],h:OxOf0b3[205]},{n:OxOf0b3[206],h:OxOf0b3[207]},{n:OxOf0b3[208],h:OxOf0b3[209]},{n:OxOf0b3[210],h:OxOf0b3[211]},{n:OxOf0b3[212],h:OxOf0b3[213]},{n:OxOf0b3[214],h:OxOf0b3[215]},{n:OxOf0b3[216],h:OxOf0b3[217]},{n:OxOf0b3[218],h:OxOf0b3[219]},{n:OxOf0b3[220],h:OxOf0b3[221]},{n:OxOf0b3[156],h:OxOf0b3[157]},{n:OxOf0b3[222],h:OxOf0b3[223]},{n:OxOf0b3[224],h:OxOf0b3[225]},{n:OxOf0b3[226],h:OxOf0b3[227]},{n:OxOf0b3[228],h:OxOf0b3[229]},{n:OxOf0b3[230],h:OxOf0b3[231]},{n:OxOf0b3[232],h:OxOf0b3[233]},{n:OxOf0b3[234],h:OxOf0b3[235]},{n:OxOf0b3[236],h:OxOf0b3[237]},{n:OxOf0b3[238],h:OxOf0b3[239]},{n:OxOf0b3[240],h:OxOf0b3[241]},{n:OxOf0b3[242],h:OxOf0b3[243]},{n:OxOf0b3[244],h:OxOf0b3[245]},{n:OxOf0b3[246],h:OxOf0b3[247]},{n:OxOf0b3[248],h:OxOf0b3[249]},{n:OxOf0b3[250],h:OxOf0b3[251]},{n:OxOf0b3[252],h:OxOf0b3[253]},{n:OxOf0b3[254],h:OxOf0b3[255]},{n:OxOf0b3[256],h:OxOf0b3[257]},{n:OxOf0b3[258],h:OxOf0b3[259]},{n:OxOf0b3[260],h:OxOf0b3[261]},{n:OxOf0b3[262],h:OxOf0b3[263]},{n:OxOf0b3[264],h:OxOf0b3[265]},{n:OxOf0b3[266],h:OxOf0b3[267]},{n:OxOf0b3[268],h:OxOf0b3[269]},{n:OxOf0b3[270],h:OxOf0b3[271]},{n:OxOf0b3[272],h:OxOf0b3[273]}];
		
		</script>
	</head>
	<body>
		<div id="container">
			<div class="tab-pane-control tab-pane" id="tabPane1">
				<div class="tab-row">
					<h2 class="tab">
						<a tabindex="-1" href='colorpicker.aspx?<%=GetDialogQueryString%>'>
							<span style="white-space:nowrap;">
								[[WebPalette]]
							</span>
						</a>
					</h2>
					<h2 class="tab selected">
							<a tabindex="-1" href='colorpicker_basic.aspx?<%=GetDialogQueryString%>'>
								<span style="white-space:nowrap;">
									[[NamedColors]]
								</span>
							</a>
					</h2>
					<h2 class="tab">
							<a tabindex="-1" href='colorpicker_more.aspx?<%=GetDialogQueryString%>'>
								<span style="white-space:nowrap;">
									[[CustomColor]]
								</span>
							</a>
					</h2>
				</div>
				<div class="tab-page">			
					<table class="colortable" align="center">
						<tr>
							<td colspan="16" height="16"><p align="left">Basic:
								</p>
							</td>
						</tr>
						<tr>
							<script>
								var OxO47fa=["length","\x3Ctd class=\x27colorcell\x27\x3E\x3Cdiv class=\x27colordiv\x27 style=\x27background-color:","\x27 title=\x27"," ","\x27 cname=\x27","\x27 cvalue=\x27","\x27\x3E\x3C/div\x3E\x3C/td\x3E",""];var arr=[];for(var i=0;i<colorlist[OxO47fa[0]];i++){arr.push(OxO47fa[1]);arr.push(colorlist[i].n);arr.push(OxO47fa[2]);arr.push(colorlist[i].n);arr.push(OxO47fa[3]);arr.push(colorlist[i].h);arr.push(OxO47fa[4]);arr.push(colorlist[i].n);arr.push(OxO47fa[5]);arr.push(colorlist[i].h);arr.push(OxO47fa[6]);} ;document.write(arr.join(OxO47fa[7]));
							</script>
						</tr>
						<tr>
							<td colspan="16" height="12"><p align="left"></p>
							</td>
						</tr>
						<tr>
							<td colspan="16"><p align="left">Additional:
								</p>
							</td>
						</tr>
						<script>
							var OxOb03d=["length","\x3Ctr\x3E","\x3Ctd class=\x27colorcell\x27\x3E\x3Cdiv class=\x27colordiv\x27 style=\x27background-color:","\x27 title=\x27"," ","\x27 cname=\x27","\x27 cvalue=\x27","\x27\x3E\x3C/div\x3E\x3C/td\x3E","\x3C/tr\x3E",""];var arr=[];for(var i=0;i<colormore[OxOb03d[0]];i++){if(i%16==0){arr.push(OxOb03d[1]);} ;arr.push(OxOb03d[2]);arr.push(colormore[i].n);arr.push(OxOb03d[3]);arr.push(colormore[i].n);arr.push(OxOb03d[4]);arr.push(colormore[i].h);arr.push(OxOb03d[5]);arr.push(colormore[i].n);arr.push(OxOb03d[6]);arr.push(colormore[i].h);arr.push(OxOb03d[7]);if(i%16==15){arr.push(OxOb03d[8]);} ;} ;if(colormore%16>0){arr.push(OxOb03d[8]);} ;document.write(arr.join(OxOb03d[9]));
						</script>
						<tr>
							<td colspan="16" height="8">
							</td>
						</tr>
						<tr>
							<td colspan="16" height="12">
								<input checked id="CheckboxColorNames" style="width: 16px; height: 20px" type="checkbox">
								<span style="width: 118px;">Use color names</span>
							</td>
						</tr>
						<tr>
							<td colspan="16" height="12">
							</td>
						</tr>
						<tr>
							<td colspan="16" valign="middle" height="24">
							<span style="height:24px;width:50px;vertical-align:middle;">Color : </span>&nbsp;
							<input type="text" id="divpreview" size="7" maxlength="7" style="width:180px;height:24px;border:#a0a0a0 1px solid; Padding:4;"/>
					
							</td>
						</tr>
				</table>
			</div>
		</div>
		<div id="container-bottom">
			<input type="button" id="buttonok" value="[[OK]]" class="formbutton" style="width:70px"	onclick="do_insert();" /> 
			&nbsp;&nbsp;&nbsp;&nbsp; 
			<input type="button" id="buttoncancel" value="[[Cancel]]" class="formbutton" style="width:70px"	onclick="do_Close();" />	
		</div>
	</div>
	</body>
</html>

