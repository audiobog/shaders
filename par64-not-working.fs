/*{
  "DESCRIPTION": "Volumetric spotlight with ISF controls for pan, tilt, zoom and dimmer.",
  "CATEGORIES": [ "Lighting", "Volumetric", "Raymarching" ],
  "INPUTS": [
    {
      "NAME": "Pan",
      "TYPE": "float",
      "MIN": -180.0,
      "MAX": 180.0,
      "DEFAULT": 0.0
    },
    {
      "NAME": "Tilt",
      "TYPE": "float",
      "MIN": -90.0,
      "MAX": 90.0,
      "DEFAULT": -45.0
    },
    {
      "NAME": "Zoom",
      "TYPE": "float",
      "MIN": 0.0,
      "MAX": 1.0,
      "DEFAULT": 0.5
    },
    {
      "NAME": "Level",
      "TYPE": "float",
      "MIN": 0.0,
      "MAX": 1.0,
      "DEFAULT": 1.0
    }
  ]
}*/

#define PI 3.14159265359
#define pos_infinite 3.402823e38
#define neg_infinite 1.4e-45

// Scene / volume parameters
float density        = 0.6;
float raysteps       = 16.0;
float lightsteps     = 12.0;
float extincting     = 0.25;
float scatteringfactor = 0.5;

// Spotlight geometry
// Fixed light position in world space
const vec3 lightpos = vec3(3.0, 3.0, -3.0);
// Thickness of volumetric region along light direction
float depth = 8.0;

// Henyey-Greenstein phase function
const float g = 0.76;
float henyey_greenstein_phase_func(float mu)
{
    return (1.0 - g * g) /
           ((4.0 * PI) * pow(1.0 + g * g - 2.0 * g * mu, 1.5));
}

// Signed distance functions
float sdSphere(vec3 p, float s)
{
    return length(p) - s;
}

float sdPlane(vec3 p)
{
    return p.y;
}

vec2 opUnion(vec2 d1, vec2 d2)
{
    return d1.x < d2.x ? d1 : d2;
}

vec2 map(in vec3 pos)
{
    vec2 res = vec2(1e10, 0.0);
    // Ground plane
    res = opUnion(res, vec2(sdPlane(pos - vec3(0.0, -1.0, 0.0)), 1.0));
    // Sphere
    res = opUnion(res, vec2(sdSphere(pos + vec3(-1.0, -1.0, 3.5), 0.4), 2.0));

    return res;
}

vec2 raycast(in vec3 origin, in vec3 direction, out float lengthHit)
{
    float len2return = -1.0;
    float lenMin = 0.0;
    float lenMax = 20.0;

    float len = lenMin;
    for (int i = 0; i < 70 && len < lenMax; ++i) {
        vec2 d = map(origin + direction * len);

        if (abs(d.x) < (0.001 * len)) {
            lengthHit = len;
            return vec2(len, d.y);
        }
        len += d.x;
    }
    lengthHit = len;
    return vec2(len2return, 1.0);
}

vec3 getnormal(in vec3 p)
{
    float h = 0.0001;
    vec2 k = vec2(1.0, -1.0);
    return normalize(
        k.xyy * map(p + k.xyy * h).x +
        k.yyx * map(p + k.yyx * h).x +
        k.yxy * map(p + k.yxy * h).x +
        k.xxx * map(p + k.xxx * h).x
    );
}

float calSoftShadow(in vec3 orign, in vec3 direction, float tMax, float kShadow)
{
    float res = 1.0;
    float t = 0.0;
    while (t < tMax) {
        vec2 d = map(orign + direction * t);
        if (d.x < 0.0001) {
            return 0.0;
        }
        res = min(res, kShadow * d.x / t);
        t += d.x;
    }
    return res;
}

// Utility: build light direction from Pan/Tilt
vec3 getLightDir()
{
    // Pan around Y axis, Tilt around X axis
    float panRad  = radians(Pan);
    float tiltRad = radians(Tilt);

    // Start pointing along -Z
    vec3 dir = vec3(0.0, 0.0, -1.0);

    // Rotate around Y (pan)
    float cp = cos(panRad);
    float sp = sin(panRad);
    dir = vec3(
        cp * dir.x + sp * dir.z,
        dir.y,
        -sp * dir.x + cp * dir.z
    );

    // Rotate around X (tilt)
    float ct = cos(tiltRad);
    float st = sin(tiltRad);
    dir = vec3(
        dir.x,
        ct * dir.y - st * dir.z,
        st * dir.y + ct * dir.z
    );

    return normalize(dir);
}

// Spotlight "k" (cosine of beam half-angle) from Zoom control
float getSpotK()
{
    // Wider beam at low Zoom, tighter at high Zoom
    // Map Zoom [0,1] -> k [0.6, 0.98]
    return mix(0.6, 0.98, Zoom);
}

vec3 render(vec3 origin, vec3 direction, out float lenHit)
{
    vec3 col = vec3(0.0);
    vec2 res = raycast(origin, direction, lenHit);

    if (res.x > 0.0) {
        vec3 objectcolor = vec3(1.0);

        if (res.y == 2.0) objectcolor = vec3(0.8, 0.0, 0.0);
        else              objectcolor = vec3(0.66, 0.66, 0.66);

        vec3 pos = origin + direction * res.x;
        vec3 normal = getnormal(pos);
        if (res.y == 1.0) normal = vec3(0.0, 1.0, 0.0);

        vec3 ambient = vec3(0.1, 0.1, 0.1);

        vec3 lightDir = lightpos - pos;
        float distance = length(lightDir);
        float attenuation = 1.0 / (1.0 + 0.14 * distance + 0.07 * distance * distance);
        lightDir = normalize(lightDir);

        vec3 spotDir = getLightDir();
        float k = getSpotK();

        // Spotlight cutoff using dot(spotDir, direction from light)
        if (dot(spotDir, -lightDir) > k) {
            float diff = attenuation * max(dot(lightDir, normal), 0.0);
            vec3 diffuse = diff * vec3(0.6, 0.6, 0.6);

            vec3 viewDir = -pos;
            vec3 halfvec = normalize(viewDir + lightDir);
            vec3 specular = attenuation *
                            pow(max(dot(halfvec, normal), 0.0), 30.0) *
                            vec3(1.0, 1.0, 1.0);
            float shadow = calSoftShadow(pos, lightDir, distance, 16.0);

            col = objectcolor * ambient + objectcolor * (diffuse + specular) * shadow;
        }
        else {
            col = objectcolor * ambient;
        }
    }
    return col;
}

bool intersect_plane(vec3 origin, vec3 dir, vec3 point, vec3 normal, out float t)
{
    float alpha = dot(dir, normal);
    if (alpha == 0.0) {
        t = pos_infinite;
        return false;
    }
    t = dot(point - origin, normal) / alpha;
    return true;
}

bool solve_quadratic(float a, float b, float c, out float t1, out float t2)
{
    float delta = b * b - 4.0 * a * c;
    if (delta < 0.0) return false;
    float temp1 = -b / (2.0 * a);
    float temp2 = sqrt(delta) / (2.0 * a);

    t1 = temp1 - temp2;
    t2 = temp1 + temp2;
    return true;
}

bool get_intersect(vec3 origin, vec3 dir, out float tmin, out float tmax)
{
    vec3 lightnormal = getLightDir();
    float k = getSpotK();

    float A = dot(origin - lightpos, lightnormal);
    float B = dot(dir, lightnormal);
    float C = dot(origin - lightpos, origin - lightpos);
    float D = dot(dir, dir);
    float E = dot(origin - lightpos, dir);

    float a = B * B - k * k * D;
    float b = 2.0 * (A * B - k * k * E);
    float c = A * A - k * k * C;
    float t1, t2;
    bool res = solve_quadratic(a, b, c, t1, t2);
    tmin = min(t1, t2);
    tmax = max(t1, t2);
    return res;
}

vec3 getsuncolor(vec3 origin, vec3 dir)
{
    float t;
    vec2 intersect = raycast(origin, dir, t);
    if (intersect.x > 0.0) return vec3(0.0);

    float tmin = 0.0;
    float tmax = length(lightpos - origin);

    float steplength = (tmax - tmin) / lightsteps;
    float transmittance = 1.0;
    float tcurrent = 0.0;

    while (tcurrent < tmax) {
        transmittance *= exp(-steplength * density * extincting);
        tcurrent += steplength;
    }
    return vec3(20.0) * transmittance;
}

vec4 raymarching(vec3 origin, vec3 dir, float tmin, float tmax, float lengthHit)
{
    if (tmin > tmax || tmax < 0.0) return vec4(0.0);
    if (tmin > lengthHit) return vec4(0.0);

    if (lengthHit > tmin && lengthHit < tmax) tmax = lengthHit;
    if (tmin < 0.0) tmin = 0.0;

    float steplength = (tmax - tmin) / raysteps;
    float transmittance = 1.0;
    vec3 res = vec3(0.0);
    float tcurrent = tmin;

    while (tcurrent < tmax) {
        vec3 pos = origin + dir * (tcurrent + 0.5 * steplength);
        transmittance *= exp(-steplength * density * extincting);

        vec3 lightDir = normalize(lightpos - pos);
        float mu = dot(-lightDir, -dir);
        float phase = henyey_greenstein_phase_func(mu);

        vec3 suncolor = getsuncolor(pos, lightDir);
        res += transmittance * scatteringfactor * density * steplength * phase * suncolor;
        tcurrent += steplength;
    }
    return vec4(res, transmittance);
}

void mainImage(out vec4 fragColor, in vec2 fragCoord)
{
    vec2 iResolution = RENDERSIZE;
    vec2 uv = (2.0 * fragCoord.xy - iResolution.xy) / iResolution.y;

    vec3 eye = vec3(0.0, 0.0, 1.0);
    vec3 lookat = vec3(0.0, 0.0, 0.0);

    vec3 front = normalize(lookat - eye);
    vec3 up = vec3(0.0, 1.0, 0.0);
    vec3 right = cross(front, up);
    up = cross(right, front);

    float focal = 1.0;

    mat3 ro = mat3(right, up, front);
    vec3 dir = normalize(vec3(uv, focal));
    dir = ro * dir;

    vec3 col = vec3(0.0);
    float t1, t2;

    float begin = 2.0, end = 1.0;

    vec3 lightnormal = getLightDir();

    if (get_intersect(eye, dir, t1, t2)) {
        float flag1 = dot(eye + dir * t1 - lightpos, lightnormal);
        float flag2 = dot(eye + dir * t2 - lightpos, lightnormal);

        float tag1 = dot(lightnormal, eye + dir * t1 - lightpos - lightnormal * depth);
        float tag2 = dot(lightnormal, eye + dir * t2 - lightpos - lightnormal * depth);

        if (flag1 < 0.0 && flag2 < 0.0) {
            // No intersection segment in front of the light
        }
        else if (flag2 < 0.0) {
            if (tag1 < 0.0) {
                end = t1;
                intersect_plane(eye, dir, lightpos + depth * lightnormal, lightnormal, begin);
            }
        }
        else if (flag1 < 0.0) {
            if (tag2 < 0.0) {
                begin = t2;
                intersect_plane(eye, dir, lightpos + depth * lightnormal, lightnormal, end);
            }
        }
        else {
            if (tag1 < 0.0 && tag2 < 0.0) {
                begin = t1;
                end = t2;
            }
            else if (tag1 > 0.0 && tag2 > 0.0) {
                // Completely behind the cap plane
            }
            else if (tag2 > 0.0) {
                begin = t1;
                intersect_plane(eye, dir, lightpos + depth * lightnormal, lightnormal, end);
            }
            else {
                intersect_plane(eye, dir, lightpos + depth * lightnormal, lightnormal, begin);
                end = t2;
            }
        }
    }

    float lengthHit;
    col = render(eye, dir, lengthHit);

    vec4 volumetriclight = raymarching(eye, dir, begin, end, lengthHit);
    col = col + volumetriclight.xyz * volumetriclight.w;

    // Apply dimmer
    col *= Level;

    fragColor = vec4(col, 1.0);
}

void main()
{
    vec2 fragCoord = isf_FragNormCoord * RENDERSIZE;
    vec4 color;
    mainImage(color, fragCoord);
    gl_FragColor = color;
}
