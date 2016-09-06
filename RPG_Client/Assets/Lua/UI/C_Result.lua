C_Result = {};
local this = C_Result;
local inst;


function this.Awake()
	inst = this.inst;
end

function this.OnFirstEnable()
	UITools.SA(inst , false)
end

function  this.Show(isWin)
	local title = '胜利'
	local desc = '恭喜您取得战斗胜利！！！！'
	if not isWin then
		title = '失败'
		desc = '非常遗憾，战斗失败，继续努力吧！！！！'
	end
	inst:GetChild('l_title').Value = title
	inst:GetChild('l_desc').Value = desc
	UITools.ShowPanel(inst)
end

function this.Cancel()
	UITools.ClosePanel(inst)
end

function this.ToMain()
	UITools.EndFightAndGoMainCity()
end