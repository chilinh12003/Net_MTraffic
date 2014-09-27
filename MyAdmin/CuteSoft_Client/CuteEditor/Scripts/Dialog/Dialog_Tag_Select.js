package Service;

import java.sql.Timestamp;
import java.util.Vector;

import MyDataSource.MyTableModel;
import Service.News.NewsType;
import Service.News.Status;

public class NewsObject implements java.io.Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	public Integer NewsID = 0;
	public Integer ServiceID = 0;
	/**
	 * Nội dung của bản tin
	 */
	public String Content = "";
	public Integer CharCount = 0;

	public News.Status mStatus = Status.NoThing;
	public News.NewsType mNewsType = NewsType.NoThing;

	/**
	 * Danh sách tên đường cho bản tin (nếu có)
	 */
	public Vector<Integer> ListStreetID = new Vector<Integer>();

	/**
	 * Thời gian push tin cho khách hàng
	 */
	public Timestamp PushTime = null;

	public boolean IsNull()
	{
		if (NewsID < 1 || ServiceID < 1 || Content == "")
			return true;
		else
			return false;
	}

	/**
	 * Lấy số MT bắn xuống cho khách hàng
	 * @return
	 */
	public Integer MTCount()
	{
		if(Content == null)
			return 0;
		Integer MTLength = Content.length();
		Integer Count = MTLength / 160;
		if(MTLength % 160 != 0)
			Count++;
		
		return Count;
	}
	
	public NewsObject()
	{

	}

	public NewsObject Convert(MyTableModel mTable) throws Exception
	{
		try
		{
			NewsObject mNewsObject = new NewsObject();

			if (mTable.IsEmpty())
				return mNewsObject;
			
			
			return mNewsObject;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     <?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<treedropdowns>
		<LinkTree>
			<item text="Msdn.microsoft.com" Selectable="False">
				<html><![CDATA[Msdn.microsoft.com <img border='0' src='http://cutesoft.net/data/msdn16.gif' />]]></html>
				<item text=".NET Framework" value="http://msdn.microsoft.com/netframework/">
					<html><![CDATA[.NET Framework]]></html>
				</item>
				<item text="ASP.NET Home" value="http://msdn.microsoft.com/asp.net/">
					<html><![CDATA[ASP.NET Home]]></html>
				</item>
			</item>
			<item text="Yahoo.com" Selectable="False">
				<html><![CDATA[Yahoo.com <img border='0' src='http://cutesoft.net/data/yahoo.gif' /> ]]></html>
				<item text="Yahoo Web" value="http://www.yahoo.com/">
					<html><![CDATA[Yahoo Web ]]></html>
				</item>
			</item>
			<item text="CuteSoft Products" Selectable="False">
				<html><![CDATA[CuteSoft Products <img border='0' src='http://cutesoft.net/data/cutesoft.gif' /> ]]></html>
				<item text="Cute Chat" value="http://cutesoft.net/ASP.NET+Chat/default.aspx">
					<html><![CDATA[Cute Chat]]></html>
				</item>
			</item>
		</LinkTree>
	</treedropdowns>
	<dropdowns>
		<CssClass>
			<item text="[[NotSet]]" value="null"></item>
			<item text="Red Text" value="RedColor">
				<html><![CDATA[<span style='color:red'>RedColor</span>]]></html>
			</item>
			<item text="textbold" value="textbold">
				<html><![CDATA[<span class='textbold'>textbold</span>]]></html>
			</item>
			<item text="Highlight" value="Highlight">
				<html><![CDATA[<span style='background-color: yellow'>Highlight</span>]]></html>
			</item>
			<item text="Bold Green Text" value="BoldGreen">
				<html><![CDATA[<span style='color: green; font-weight: bold;'>Bold Green Text</span>]]></html>
			</item>
		</CssClass>
		<CssStyle>
			<item text="[[NotSet]]" value="null"></item>
			<item text="font-size:18pt" value="font-size:18pt"></item>
			<item text="color:red" value="color:red"></item>
			<item text="border:1px red solid" value="border:1px red solid"></item>
			<!--<item text="filter:alpha(opacity=20)">filter:alpha(opacity=20)</item> -->
		</CssStyle>
		<Codes>
			<item text="Email signature">
				<value><![CDATA[<h3>this is my email signature</h3>]]></value>
				<html><![CDATA[<img border='0' src='http://cutesoft.net/data/signature.gif' /> Email signature]]></html>
			</item>
			<item text="Contact us">
				<value><![CDATA[<a href="mailto:support@CuteSoft.Net">Contact us</a>]]></value>
				<html><![CDATA[<img border='0' src='http://cutesoft.net/data/contact.gif' /> Contact us]]></html>
			</item>
		</Codes>
		<Links>
			<item text="CuteSoft" value="http://cutesoft.net/">
				<html><![CDATA[<img border='0' src='http://cutesoft.net/data/signature.gif' /> CuteSoft]]></html>
			</item>
			<item text="Mail to us" value="mailto:support@CuteSoft.Net">
				<html><![CDATA[<img border='0' src='http://cutesoft.net/data/email.gif' /> Mail to us]]></html>
			</item>
			<item text="Yahoo.com" value="http://www.yahoo.com/">
				<html><![CDATA[<img border='0' src='http://cutesoft.net/data/yahoo.gif' /> Yahoo.com]]></html>
			</item>
			<item text="Google.com" value="http://www.google.com">
				<html><![CDATA[<img border='0' src='http://cutesoft.net/data/Google.gif' /> Google.com]]></html>
			</item>
			<item text="MSDN