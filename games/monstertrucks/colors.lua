
_colors = {
   black = {0,0,0},
   red = {0xDC,0x14,0x3C},
   orange = {0xFF,0x8C,0x00},
   yellow = {0xFF,0xEB, 0x00},
   green = {0x32,0xCD,0x32},
   blue = {0x18,0x7A,0xD3},
   purple = {0xAA,0x55, 0xC2},
   gray = {0x8d,0x8d,0x8d},
   grey = {0x8d,0x8d,0x8d},
   white = {0xFF,0xFF,0xFF},
   brown = {0xCD,0x85,0x3F}
}

function _applyShade(c,shade)
   if shade == nil then shade = 0 end
   local H,S,L = _rgb2hsl(unpack(c))
   L = L + shade
   if L > 1 then L = 1 end
   if L < 0 then L = 0 end
   return {_hsl2rgb(H,S,L)}
end

--http://130.113.54.154/~monger/hsl-rgb.html

-- take r,g,b from 0 to 255
-- return h from 0 to 2pi
-- return s,l from 0 to 1
function _rgb2hsl(r,g,b)
	R,G,B = r/255,g/255,b/255
	local maxcolor = math.max(R,G,B)
	local mincolor = math.min(R,G,B)
	
	local H,S,L
	L = (maxcolor + mincolor)/2
	if maxcolor == mincolor then
	   return 0,0,L
	end
	
	if L < 0.5 then
	   S = (maxcolor - mincolor) / (maxcolor + mincolor)
	else
	   S = (maxcolor - mincolor) / (2 - maxcolor - mincolor)
	end
	
	if R == maxcolor then
	   H = (G-B) / (maxcolor-mincolor)
	end
	if G == maxcolor then
	   H = 2 + (B-R)/(maxcolor-mincolor)
	end
	if B == maxcolor then
	   H = 4 + (R-G)/(maxcolor-mincolor)
	end
	return H,S,L
end

-- take h from 0 to 2pi
-- take s,l from 0 to 1
-- return r,g,b from 0 to 255
function _hsl2rgb(h,s,l)
   local H,S,L = h/(2*math.pi),s,l
   if S == 0 then
      return L*255,L*255,L*255
   end
   
   local temp1,temp2
   
   if L < 0.5 then
      temp2 = L* (1+S)
   else
      temp2 = L + S - L*S
   end
   
   temp1 = 2*L - temp2
   
   local temp3c = {r=H+1/3, g=H, b=H-1/3}
   local c = {r=0,g=0,b=0}
   
   for i,v in pairs(temp3c) do
      local temp3 = v
      if v < 0 then temp3 = v + 1 end
      if v > 1 then temp3 = v - 1 end
      
      if 6*temp3 < 1 then
         c[i] = temp1+(temp2-temp1)*6*temp3
      elseif 2*temp3 < 1 then
         c[i] = temp2
      elseif 3*temp3 < 2 then
         c[i] = temp1+(temp2-temp1)*((2/3)-temp3)*6
      else
         c[i] = temp1
      end
   end
   
   return c.r*255,c.g*255,c.b*255
end

--[[displays our preset palette
function drawcolors()
   local y = 10
   local s = 25
   local pad = 10
   for i,color in pairs(_colors) do
      setcolor(color,light)
   	  fillrect(pad, y, s,s)
   	  setcolor(color)
   	  fillrect(pad*2+s,y,s,s)
   	  setcolor(color,dark)
   	  fillrect(pad*3+2*s,y,s,s)
   	  y = y + s + pad
   end
end]]

function _colorFromName(name)
   name = string.gsub(name,"%s+","")
   
   local min = 0
   local max = #name
   
   local shade = 0
   
   if string.find(name,"^light") then
      min = 6
      shade = 0.2
   elseif string.find(name,"^dark") then
      min = 5
      shade = -0.2
   end
   
   local opacity = 255
   
   if string.find(name,"glass$") then
      max = -6
      opacity = 128
   end
   
   name = string.sub(name,min,max)
   
   local c = _applyShade(_colors[name], shade)
   
   return c[1], c[2], c[3], opacity
   
end

background = function(c) love.graphics.setBackgroundColor(_colorFromName(c)) end

color = function(c) love.graphics.setColor(_colorFromName(c)) end