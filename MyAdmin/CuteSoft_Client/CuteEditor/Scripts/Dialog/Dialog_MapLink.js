var OxO9cb2=["inp_src","inp_title","inp_target","sel_protocol","btnbrowse","editor","","href","value","title","target","onclick","length","options","://",":","others","selectedIndex"];var inp_src=Window_GetElement(window,OxO9cb2[0],true);var inp_title=Window_GetElement(window,OxO9cb2[1],true);var inp_target=Window_GetElement(window,OxO9cb2[2],true);var sel_protocol=Window_GetElement(window,OxO9cb2[3],true);var btnbrowse=Window_GetElement(window,OxO9cb2[4],true);var obj=Window_GetDialogArguments(window);var editor=obj[OxO9cb2[5]];SyncToView();function SyncToView(){var src=obj[OxO9cb2[7]].replace(/$\s*/,OxO9cb2[6]);Update_sel_protocol(src);inp_src[OxO9cb2[8]]=src;if(obj[OxO9cb2[9]]){inp_title[OxO9cb2[8]]=obj[OxO9cb2[9]];} ;if(obj[OxO9cb2[10]]){inp_target[OxO9cb2[8]]=obj[OxO9cb2[10]];} ;} ;btnbrowse[OxO9cb2[11]]=function btnbrowse_onclick(){function Ox354(Ox137){if(Ox137){inp_src[OxO9cb2[8]]=Ox137;} ;} ;editor.SetNextDialogWindow(window);if(Browser_IsSafari()){editor.ShowSelectFileDialog(Ox354,inp_src.value,inp_src);} else {editor.ShowSelectFileDialog(Ox354,inp_src.value);} ;} ;function sel_protocol_change(){var src=inp_src[OxO9cb2[8]].replace(/$\s*/,OxO9cb2[6]);for(var i=0;i<sel_protocol[OxO9cb2[13]][OxO9cb2[12]];i++){var Ox13b=sel_protocol[OxO9cb2[13]][i][OxO9cb2[8]];if(src.substr(0,Ox13b.length).toLowerCase()==Ox13b){src=src.substr(Ox13b.length,src[OxO9cb2[12]]-Ox13b[OxO9cb2[12]]);break ;} ;} ;var Ox43b=src.indexOf(OxO9cb2[14]);if(Ox43b!=-1){src=src.substr(Ox43b+3,src[OxO9cb2[12]]-3-Ox43b);} ;var Ox43b=src.indexOf(OxO9cb2[15]);if(Ox43b!=-1){src=src.substr(Ox43b+1,src[OxO9cb2[12]]-1-Ox43b);} ;var Ox43c=sel_protocol[OxO9cb2[8]];if(Ox43c==OxO9cb2[16]){Ox43c=OxO9cb2[6];} ;inp_src[OxO9cb2[8]]=Ox43c+src;} ;function Update_sel_protocol(src){var Ox43e=false;for(var i=0;i<sel_protocol[OxO9cb2[13]][OxO9cb2[12]];i++){var Ox13b=sel_protocol[OxO9cb2[13]][i][OxO9cb2[8]];if(src.substr(0,Ox13b.length).toLowerCase()==Ox13b){if(sel_protocol[OxO9cb2[17]]!=i){sel_protocol[OxO9cb2[17]]=i;} ;Ox43e=true;break ;} ;} ;if(!Ox43e){sel_protocol[OxO9cb2[17]]=sel_protocol[OxO9cb2[13]][OxO9cb2[12]]-1;} ;} ;function insert_link(){var arr= new Array();arr[0]=inp_src[OxO9cb2[8]];if(inp_target[OxO9cb2[8]]){arr[1]=inp_target[OxO9cb2[8]];} ;if(inp_title[OxO9cb2[8]]){arr[2]=inp_title[OxO9cb2[8]];} ;Window_SetDialogReturnValue(window,arr);Window_CloseDialog(window);} ;