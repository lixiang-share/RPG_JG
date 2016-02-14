C_SelectServer_C_StartGame = {};
local this = C_SelectServer_C_StartGame;
local inst;


function this.Awake()
	inst = this.inst
end

function this.OnFirstEnable()
	UITools.SA(inst,false)
end

function this.OnEnable()
	inst:CreateMsg():SetMsgType(MsgProtocol.Get_ServerList):Send()
end

function this.OnReceiveData(unpacker)
	local list = UITools.MsgToServerList(unpacker)
	UITools.Log(list.Count)
	this.SetCurSelect(list:get_Item(0))
	UITools.SA(inst:GetChild('Scroll View'), true)
	inst:GetChild('Scroll View'):Parse(list)
end

function this.SetCurSelect(data)
	inst:S('curSelect' ,data)
	local item = inst:GetChild('b_curSelect')
	local desc = ' '..data.Id .. '区 '.. data.Name
	UITools.Set(item:Child('l_name') , desc)
	if data.Count > 20 then
		UITools.Set(item:Child('s_bg'),'s_btn_hot02')
	else
		UITools.Set(item:Child('s_bg'),'s_btn_smooth02')
	end
end

function this.EnterGame()
	UITools.ClosePanel(inst);
	UITools.D('EnterGame'):S('curServer' , inst:G('curSelect'))
	UITools.ShowPanel(UITools.D('EnterGame'))
end
function this.Cancel( )
	UITools.ClosePanel(inst);
	UITools.ShowPanel(UITools.D('Login'))
end

