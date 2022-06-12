<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.Order" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
<div class="order">
    <table>
        <tr>
            <td>Departure Date</td>
            <td><asp:TextBox ID="textBoxDepartureDate" runat="server"></asp:TextBox>
            <ajax:CalendarExtender ID="calendarDepartureDate" runat="server" TargetControlID="textBoxDepartureDate" Format="dd/MM/yyyy"></ajax:CalendarExtender></td>
        </tr>
        <tr>
            <td>Your flexiability</td>
            <td><asp:TextBox ID="textBoxFlexibility" runat="server" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Adult passenger</td>
            <td><asp:DropDownList ID="ddlNumberOfPeople" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Infant (age 0-2)</td>
            <td><asp:DropDownList ID="ddlInfants" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Children (age 2-12)</td>
            <td><asp:DropDownList ID="ddlChildren" runat="server"></asp:DropDownList></td>            
        </tr>
        <tr>
            <td>Special request (hotels, restaurants or flight)</td>
            <td><asp:TextBox ID="textBoxSpecialRequest" runat="server" TextMode="MultiLine"></asp:TextBox></td>
        </tr>               
    </table>
<h2>Contact information</h2>
<table cellspacing="0" cellpadding="3" width="100%" border="0">
    <tbody>
        <tr>
            <td width="120" valign="top" align="left" class="normal">Full name<span class="style1">[ &bull; ]</span></td>
            <td valign="top" align="left" class="normal">
            <table cellspacing="0" cellpadding="1" border="0">
                <tbody>
                    <tr>
                        <td><label>                               <select name="gender" id="gender">
                        <option value="Mr">Mr</option>
                        <option value="Mrs">Mrs</option>
                        <option value="Ms">Ms</option>
                        </select>                             </label></td>
                        <td><input type="text" name="hoten" class="form" id="hoten" size="37" value="" /></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" class="normal">Email <span class="style1">[ &bull; ]</span></td>
            <td valign="top" align="left" class="normal"><input type="text" name="email" class="form" id="email" size="37" value="" /></td>
        </tr>
        <tr>
            <td width="120" valign="top" align="left" class="normal">Address <span class="style1">[ &bull; ]</span></td>
            <td valign="top" align="left" class="normal"><input type="text" name="address" class="form" id="address" size="37" value="" /></td>
        </tr>
        <tr>
            <td valign="top" align="left" class="normal">City<span class="style1">[ &bull; ]</span></td>
            <td valign="top" align="left" class="normal"><input type="text" name="thanhpho" class="form" id="thanhpho" size="37" value="" /></td>
        </tr>
        <tr>
            <td valign="top" align="left" class="normal">Country</td>
            <td valign="top" align="left" class="normal"><select name="quocgia" class="form">
            <option value="0">                             N/A                            </option>
            <option value="1">                             Afghanistan                            </option>
            <option value="2">                             Albania                            </option>
            <option value="3">                             Algeria                            </option>
            <option value="4">                             American Samoa                            </option>
            <option value="5">                             Andorra                            </option>
            <option value="6">                             Angola                            </option>
            <option value="7">                             Anguilla                            </option>
            <option value="8">                             Antarctica                            </option>
            <option value="10">                             Argentina                            </option>
            <option value="11">                             Armenia                            </option>
            <option value="12">                             Aruba                            </option>
            <option value="13">                             Australia                            </option>
            <option value="14">                             Austria                            </option>
            <option value="15">                             Azerbaijan                            </option>
            <option value="16">                             Bahamas                            </option>
            <option value="17">                             Bahrain                            </option>
            <option value="18">                             Bangladesh                            </option>
            <option value="19">                             Barbados                            </option>
            <option value="20">                             Belarus                            </option>
            <option value="21">                             Belgium                            </option>
            <option value="22">                             Belize                            </option>
            <option value="23">                             Benin                            </option>
            <option value="24">                             Bermuda                            </option>
            <option value="25">                             Bhutan                            </option>
            <option value="26">                             Bolivia                            </option>
            <option value="28">                             Botswana                            </option>
            <option value="29">                             Bouvet Island                            </option>
            <option value="30">                             Brazil                            </option>
            <option value="32">                             Brunei Darussalam                            </option>
            <option value="33">                             Bulgaria                            </option>
            <option value="34">                             Burkina Faso                            </option>
            <option value="35">                             Burundi                            </option>
            <option value="36">                             Cambodia                            </option>
            <option value="37">                             Cameroon                            </option>
            <option value="38">                             Canada                            </option>
            <option value="39">                             Cape Verde                            </option>
            <option value="40">                             Cayman Islands                            </option>
            <option value="42">                             Chad                            </option>
            <option value="43">                             Chile                            </option>
            <option value="44">                             China                            </option>
            <option value="45">                             Christmas Island                            </option>
            <option value="47">                             Colombia                            </option>
            <option value="48">                             Comoros                            </option>
            <option value="49">                             Congo                            </option>
            <option value="50">                             Congo                            </option>
            <option value="51">                             Cook Islands                            </option>
            <option value="52">                             Costa Rica                            </option>
            <option value="53">                             Cote Divoire                            </option>
            <option value="54">                             Croatia                            </option>
            <option value="55">                             Cuba                            </option>
            <option value="56">                             Cyprus                            </option>
            <option value="57">                             Czech Republic                            </option>
            <option value="58">                             Denmark                            </option>
            <option value="59">                             Djibouti                            </option>
            <option value="60">                             Dominica                            </option>
            <option value="61">                             Dominican Republic                            </option>
            <option value="62">                             East Timor                            </option>
            <option value="63">                             Ecuador                            </option>
            <option value="64">                             Egypt                            </option>
            <option value="65">                             El Salvador                            </option>
            <option value="66">                             Equatorial Guinea                            </option>
            <option value="67">                             Eritrea                            </option>
            <option value="68">                             Estonia                            </option>
            <option value="69">                             Ethiopia                            </option>
            <option value="70">                             Falkland Islands                            </option>
            <option value="71">                             Faroe Islands                            </option>
            <option value="72">                             Fiji                            </option>
            <option value="73">                             Finland                            </option>
            <option value="74">                             France                            </option>
            <option value="78">                             Gabon                            </option>
            <option value="79">                             Gambia                            </option>
            <option value="80">                             Georgia                            </option>
            <option value="81">                             Germany                            </option>
            <option value="82">                             Ghana                            </option>
            <option value="83">                             Gibraltar                            </option>
            <option value="84">                             Greece                            </option>
            <option value="85">                             Greenland                            </option>
            <option value="86">                             Grenada                            </option>
            <option value="87">                             Guadeloupe                            </option>
            <option value="88">                             Guam                            </option>
            <option value="89">                             Guatemala                            </option>
            <option value="90">                             Guinea                            </option>
            <option value="91">                             Guinea-Bissau                            </option>
            <option value="92">                             Guyana                            </option>
            <option value="93">                             Haiti                            </option>
            <option value="95">                             Holy                            </option>
            <option value="96">                             Honduras                            </option>
            <option value="97">                             Hong Kong                            </option>
            <option value="98">                             Hungary                            </option>
            <option value="99">                             Iceland                            </option>
            <option value="100">                             India                            </option>
            <option value="101">                             Indonesia                            </option>
            <option value="102">                             Iran                            </option>
            <option value="103">                             Iraq                            </option>
            <option value="104">                             Ireland                            </option>
            <option value="105">                             Israel                            </option>
            <option value="106">                             Italy                            </option>
            <option value="107">                             Jamaica                            </option>
            <option value="108">                             Japan                            </option>
            <option value="109">                             Jordan                            </option>
            <option value="110">                             Kazakstan                            </option>
            <option value="111">                             Kenya                            </option>
            <option value="112">                             Kiribati                            </option>
            <option value="113">                             Korea, North                            </option>
            <option value="114">                             Korea, South                            </option>
            <option value="115">                             Kuwait                            </option>
            <option value="116">                             Kyrgyzstan                            </option>
            <option value="117">                             Laos                            </option>
            <option value="118">                             Latvia                            </option>
            <option value="119">                             Lebanon                            </option>
            <option value="120">                             Lesotho                            </option>
            <option value="121">                             Liberia                            </option>
            <option value="123">                             Liechtenstein                            </option>
            <option value="124">                             Lithuania                            </option>
            <option value="125">                             Luxembourg                            </option>
            <option value="126">                             Macau                            </option>
            <option value="127">                             Macedonia                            </option>
            <option value="128">                             Madagascar                            </option>
            <option value="129">                             Malawi                            </option>
            <option value="130">                             Malaysia                            </option>
            <option value="131">                             Maldives                            </option>
            <option value="132">                             Mali                            </option>
            <option value="133">                             Malta                            </option>
            <option value="134">                             Marshall Islands                            </option>
            <option value="135">                             Martinique                            </option>
            <option value="136">                             Mauritania                            </option>
            <option value="137">                             Mauritius                            </option>
            <option value="138">                             Mayotte                            </option>
            <option value="139">                             Mexico                            </option>
            <option value="140">                             Micronesia                            </option>
            <option value="141">                             Moldova                            </option>
            <option value="142">                             Monaco                            </option>
            <option value="143">                             Mongolia                            </option>
            <option value="144">                             Montserrat                            </option>
            <option value="145">                             Morocco                            </option>
            <option value="146">                             Mozambique                            </option>
            <option value="147">                             Myanmar                            </option>
            <option value="148">                             Namibia                            </option>
            <option value="149">                             Nauru                            </option>
            <option value="150">                             Nepal                            </option>
            <option value="151">                             Netherlands                            </option>
            <option value="152">                             Netherlands Antilles                            </option>
            <option value="153">                             New Caledonia                            </option>
            <option value="154">                             New Zealand                            </option>
            <option value="155">                             Nicaragua                            </option>
            <option value="156">                             Niger                            </option>
            <option value="157">                             Nigeria                            </option>
            <option value="158">                             Niue                            </option>
            <option value="159">                             Norfolk Island                            </option>
            <option value="161">                             Norway                            </option>
            <option value="162">                             Oman                            </option>
            <option value="163">                             Pakistan                            </option>
            <option value="164">                             Palau                            </option>
            <option value="165">                             Palestinian                            </option>
            <option value="166">                             Panama                            </option>
            <option value="167">                             Papua New Guinea                            </option>
            <option value="168">                             Paraguay                            </option>
            <option value="169">                             Peru                            </option>
            <option value="170">                             Philippines                            </option>
            <option value="171">                             Pitcairn                            </option>
            <option value="172">                             Poland                            </option>
            <option value="173">                             Portugal                            </option>
            <option value="174">                             Puerto Rico                            </option>
            <option value="175">                             Qatar                            </option>
            <option value="176">                             R&eacute;Union                            </option>
            <option value="177">                             Romania                            </option>
            <option value="178">                             Russian Federation                            </option>
            <option value="179">                             Rwanda                            </option>
            <option value="180">                             Saint Helena                            </option>
            <option value="182">                             Saint Lucia                            </option>
            <option value="183">                             Saint Pierre                            </option>
            <option value="184">                             Saint Vincent                            </option>
            <option value="185">                             Samoa                            </option>
            <option value="186">                             San Marino                            </option>
            <option value="188">                             Saudi Arabia                            </option>
            <option value="189">                             Senegal                            </option>
            <option value="190">                             Seychelles                            </option>
            <option value="191">                             Sierra Leone                            </option>
            <option value="192">                             Singapore                            </option>
            <option value="193">                             Slovakia                            </option>
            <option value="194">                             Slovenia                            </option>
            <option value="195">                             Solomon Islands                            </option>
            <option value="196">                             Somalia                            </option>
            <option value="197">                             South Africa                            </option>
            <option value="198">                             South Georgia                            </option>
            <option value="199">                             Spain                            </option>
            <option value="200">                             Sri Lanka                            </option>
            <option value="201">                             Sudan                            </option>
            <option value="202">                             Suriname                            </option>
            <option value="204">                             Swaziland                            </option>
            <option value="205">                             Sweden                            </option>
            <option value="206">                             Switzerland                            </option>
            <option value="208">                             Taiwan                            </option>
            <option value="209">                             Tajikistan                            </option>
            <option value="210">                             Tanzania                            </option>
            <option value="211">                             Thailand                            </option>
            <option value="212">                             Togo                            </option>
            <option value="213">                             Tokelau                            </option>
            <option value="214">                             Tonga                            </option>
            <option value="216">                             Tunisia                            </option>
            <option value="217">                             Turkey                            </option>
            <option value="218">                             Turkmenistan                            </option>
            <option value="220">                             Tuvalu                            </option>
            <option value="221">                             Uganda                            </option>
            <option value="222">                             Ukraine                            </option>
            <option value="224">                             United Kingdom                            </option>
            <option value="225">                             United States                            </option>
            <option value="227">                             Uruguay                            </option>
            <option value="226">                             Usa Minor Islands                            </option>
            <option value="228">                             Uzbekistan                            </option>
            <option value="229">                             Vanuatu                            </option>
            <option value="230">                             Vatican City State                            </option>
            <option value="231">                             Venezuela                            </option>
            <option value="232" selected="selected">                             Vietnam                            </option>
            <option value="235">                             Wallis And Futuna                            </option>
            <option value="236">                             Western Sahara                            </option>
            <option value="237">                             Yemen                            </option>
            <option value="238">                             Yugoslavia                            </option>
            <option value="239">                             Zaire                            </option>
            <option value="240">                             Zambia                            </option>
            <option value="241">                             Zimbabwe                            </option>
            </select></td>
        </tr>
        <tr>
            <td width="120" valign="top" align="left" class="normal">Telephone<span class="style1">[ &bull; ]</span></td>
            <td valign="top" align="left" class="normal"><input type="text" name="dienthoai" class="form" id="dienthoai" size="15" value="" /></td>
        </tr>
        <tr>
            <td width="120" valign="top" align="left" class="normal">Mobile phone<span class="style1">[ &bull; ]</span></td>
            <td valign="top" align="left" class="normal"><input type="text" name="phone" class="form" id="phone" size="15" value="" /></td>
        </tr>
    </tbody>
</table>
<p><em><br />
The information has <span class="normal"><span class="style1">[ &bull; ]</span> </span> is required!</em></p>  
    <asp:Button ID="buttonSubmit" runat="server" OnClick="buttonSubmit_Click" Text="Submit"/>
</div>