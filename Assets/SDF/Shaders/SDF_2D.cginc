#ifndef SDF_2D
#define 2DF_2D

float map(float x, float inMin, float inMax, float outMin, float outMax)
{
    return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
}

float2 translate(float2 position, float2 offset)
{
    return position - offset;
}

float2 scale(float2 position, float scale)
{
    return position / scale;
}

fixed4 debugColor(float distance)
{
    fixed3 col = (distance > 0.0) ? fixed3(0.9,0.6,0.3) : fixed3(0.65,0.85,1.0);
    col *= 1.0 - exp(-6.0 * abs(distance));
    col *= 0.8 + 0.2 * cos(150.0 * distance);
    col = lerp(col, fixed3(1.0, 1.0, 1.0), 1.0 - smoothstep(0.0,0.01, abs(distance)));
    return fixed4(col, 1.0);
}

float circle(float2 position, float fill, float smoothness)
{
    const float thickness = map(fill, 0.0, 1.0, -smoothness, 0.5);
    return abs(length(position) - (1.0 - smoothness) + thickness)  - thickness;
}

float rectangle(float2 position, float2 b,  float cornerRadius, float fill, float smoothness)
{
    b.x -= smoothness;
    b.y -= smoothness;
    const float thickness = map(fill, 0.0, 1.0, -smoothness, 0.5);
    float2 q = abs(position) - b + cornerRadius;
    float temp = min(max(q.x, q.y), 0.0) + length(max(q, 0.0)) - cornerRadius;
    float distance = abs(temp + thickness) - thickness;
    return distance;
}

float equitriangle(float2 position, float cornerRadius,float fill, float smoothness)
{
    float size = 1.0 - smoothness - cornerRadius;
    const float thickness = map(fill, 0.0, 1.0, -smoothness, (sqrt(3.0) * (0.5f) / 3.0) + 0.2113 * cornerRadius);
    const float height_over_radius = sqrt(3.0);
    position.x = abs(position.x) - size;
    position.y = position.y + size * height_over_radius - size;
    if( position.x + height_over_radius * position.y > 0.0 ) position = float2(position.x-height_over_radius*position.y,-height_over_radius*position.x-position.y)/2.0;
    position.x -= clamp( position.x, -2.0*size, 0.0 );
    return abs(-length(position)*sign(position.y) + thickness - cornerRadius) - thickness;
}

fixed4 debugColors(fixed4 insideColor, fixed4 outsideColor, float distance, float lineDistance, float lineThickness, float _sublines, float sublineThickness)
{
    fixed4 col = lerp(insideColor, outsideColor, distance);
    
    float distanceChange = fwidth(distance) * 0.5;
    float majorLineDistance = abs(frac(distance / lineDistance + 0.5) - 0.5) * lineDistance;
    float majorLines = smoothstep(lineThickness - distanceChange, lineThickness + distanceChange, majorLineDistance);

    float distanceBetweenSubLines = lineDistance / _sublines;
    float subLineDistance = abs(frac(distance / distanceBetweenSubLines + 0.5) - 0.5) * distanceBetweenSubLines;
    float subLines = smoothstep(sublineThickness - distanceChange, sublineThickness + distanceChange, subLineDistance);

    return col * majorLines * subLines;
    
}

#endif