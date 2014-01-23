_circleSegs = 64

_fill = true
fill = function(on) _fill = on end

smooth = function(on)
	if on then
		love.graphics.setLineStyle("smooth")
	else
		love.graphics.setLineStyle("rough")
	end
end

thick = function(n)
	love.graphics.setLineWidth(n)
end

line = function(x1,y1,x2,y2,...)
	love.graphics.line(x1,y1,x2,y2,...)
end

box = function(left,top,width,height)
	local right = left + width
	local bottom = top + height
	if _fill then
		love.graphics.rectangle("fill", left, top, width, height)
	else
		line(left,top,right,top,right,bottom,left,bottom,left,top)
	end
end

circle = function(cx,cy,r)
	if _fill then
		love.graphics.circle("fill",cx,cy,r,_circleSegs)
	else
		love.graphics.circle("line",cx,cy,r,_circleSegs)
	end
end

oval = function(cx,cy,px1,py1,px2,py2)
	
end

shape = function(...)
	if _fill then
		love.graphics.polygon("fill",...)
	else
		line(...)
	end
end