void Tone_float(float3 In, float bright, out float3 Out)
{
    const half a = bright;
    const half b = 0.03;
    const half c = 2.43;
    const half d = 0.59;
    const half e = 0.14;
    Out = saturate((In * (a * In + b)) / (In * (c * In + d) + e));
}

void Tone_half(half3 In, float bright, out half3 Out)
{
    const half a = bright;
    const half b = 0.03;
    const half c = 2.43;
    const half d = 0.59;
    const half e = 0.14;
    Out = saturate((In * (a * In + b)) / (In * (c * In + d) + e));
}

void ToneB_float(float3 In, float bright, out float3 Out)
{
    const half a = bright;
    const half b = 0.05;
    const half c = 2.0;
    const half d = 0.7;
    const half e = 0.1;
    Out = saturate((In * (a * In + b)) / (In * (c * In + d) + e));
}

void ToneB_half(half3 In, float bright, out half3 Out)
{
    const half a = bright;
    const half b = 0.05;
    const half c = 2.0;
    const half d = 0.7;
    const half e = 0.1;
    Out = saturate((In * (a * In + b)) / (In * (c * In + d) + e));
}
