
_textsizelarge = 100
_textsizesmall = 30
_mastertextsize = _textsizesmall
_textsize = 0
_textscale = 0
_textleft = 0
_textright = 0
_texttop = 0
_textalign = "left"

_fontFile = "Key Comic.ttf"
_fontlarge = love.graphics.newFont(_fontFile, _textsizelarge)
_fontsmall = love.graphics.newFont(_fontFile, _textsizesmall)
_font = _fontlarge

textstart = function(left,top,width)
	_textleft = left
	if width == nil then
		_textright = 800
	else
		_textright = left+width
	end
	_texttop = top
end

textalign = function(align)
	_textalign = align
end

textsize = function(size)
	if size <= _textsizesmall then
		_mastertextsize = _textsizesmall
		_font = _fontsmall
	else
		_mastertextsize = _textsizelarge
		_font = _fontlarge
	end
	love.graphics.setFont(_font)
	_textsize = size
	_textscale = size / _mastertextsize
end

text = function(s)
	local q = _textscale
	_texttop = _texttop + _font:getHeight() * q
	love.graphics.push()
	love.graphics.scale(q,q)
	love.graphics.printf(s, _textleft/q, _texttop/q, (_textright - _textleft)/q, _textalign)
	love.graphics.pop()
end

textsize(30)