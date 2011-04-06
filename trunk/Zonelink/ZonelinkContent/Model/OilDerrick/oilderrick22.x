xof 0303txt 0032
template Vector {
 <3d82ab5e-62da-11cf-ab39-0020af71e433>
 FLOAT x;
 FLOAT y;
 FLOAT z;
}

template MeshFace {
 <3d82ab5f-62da-11cf-ab39-0020af71e433>
 DWORD nFaceVertexIndices;
 array DWORD faceVertexIndices[nFaceVertexIndices];
}

template Mesh {
 <3d82ab44-62da-11cf-ab39-0020af71e433>
 DWORD nVertices;
 array Vector vertices[nVertices];
 DWORD nFaces;
 array MeshFace faces[nFaces];
 [...]
}

template MeshNormals {
 <f6f23f43-7686-11cf-8f52-0040333594a3>
 DWORD nNormals;
 array Vector normals[nNormals];
 DWORD nFaceNormals;
 array MeshFace faceNormals[nFaceNormals];
}

template Coords2d {
 <f6f23f44-7686-11cf-8f52-0040333594a3>
 FLOAT u;
 FLOAT v;
}

template MeshTextureCoords {
 <f6f23f40-7686-11cf-8f52-0040333594a3>
 DWORD nTextureCoords;
 array Coords2d textureCoords[nTextureCoords];
}

template ColorRGBA {
 <35ff44e0-6c7c-11cf-8f52-0040333594a3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
 FLOAT alpha;
}

template ColorRGB {
 <d3e16e81-7835-11cf-8f52-0040333594a3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
}

template Material {
 <3d82ab4d-62da-11cf-ab39-0020af71e433>
 ColorRGBA faceColor;
 FLOAT power;
 ColorRGB specularColor;
 ColorRGB emissiveColor;
 [...]
}

template MeshMaterialList {
 <f6f23f42-7686-11cf-8f52-0040333594a3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material <3d82ab4d-62da-11cf-ab39-0020af71e433>]
}

template TextureFilename {
 <a42790e1-7810-11cf-8f52-0040333594a3>
 STRING filename;
}


Mesh {
 301;
 3.207814;0.000000;-7.766968;,
 9.159991;0.000000;-7.766968;,
 -0.722186;0.000000;7.996673;,
 9.159991;0.000000;7.996673;,
 3.207814;3.300000;-7.766968;,
 9.159991;3.300000;-7.766968;,
 -0.722186;3.300000;7.996673;,
 9.159991;3.300000;7.996673;,
 -0.722186;0.000000;-3.836969;,
 -0.722186;3.300000;-3.836969;,
 1.488439;0.000000;-7.521344;,
 0.260314;0.000000;-6.784468;,
 -0.476561;0.000000;-5.556344;,
 -0.476561;3.300000;-5.556344;,
 0.260314;3.300000;-6.784468;,
 1.488439;3.300000;-7.521344;,
 5.414144;-0.030768;1.602901;,
 3.039181;-0.030768;0.927763;,
 1.653824;-0.030768;-1.116020;,
 1.906298;-0.030768;-3.572138;,
 3.678466;-0.030768;-5.291347;,
 6.141112;-0.030768;-5.469209;,
 8.141936;-0.030768;-4.022501;,
 8.744729;-0.030768;-1.628156;,
 7.667442;-0.030768;0.593489;,
 5.414144;9.681529;1.602901;,
 3.039181;9.681529;0.927763;,
 1.653824;9.681529;-1.116020;,
 1.906298;9.681529;-3.572138;,
 3.678466;9.681529;-5.291347;,
 6.141112;9.681529;-5.469209;,
 8.141935;9.681529;-4.022501;,
 8.744729;9.681529;-1.628156;,
 7.667442;9.681529;0.593489;,
 5.337307;11.284681;0.539058;,
 3.664146;11.284681;0.063423;,
 2.688162;11.284681;-1.376422;,
 2.866030;11.284681;-3.106758;,
 4.114522;11.284681;-4.317941;,
 5.849457;11.284681;-4.443245;,
 7.259038;11.284681;-3.424040;,
 7.683707;11.284681;-1.737222;,
 6.924757;11.284681;-0.172072;,
 5.228184;12.379650;-0.971842;,
 4.551739;12.379650;-1.164136;,
 4.157158;12.379650;-1.746253;,
 4.229068;12.379650;-2.445812;,
 4.733823;12.379650;-2.935483;,
 5.435241;12.379650;-2.986142;,
 6.005123;12.379650;-2.574086;,
 6.176813;12.379650;-1.892121;,
 5.869976;12.379650;-1.259345;,
 5.184575;12.531077;-1.957335;,
 5.635654;-0.030768;0.776773;,
 7.823530;-0.030768;1.921090;,
 8.763986;-0.030768;4.204027;,
 8.016973;-0.030768;6.557372;,
 5.932028;-0.030768;7.879969;,
 3.484719;-0.030768;7.552958;,
 1.820169;-0.030768;5.729355;,
 1.717239;-0.030768;3.262441;,
 3.224092;-0.030768;1.306514;,
 5.635654;9.681529;0.776773;,
 7.823531;9.681529;1.921091;,
 8.763986;9.681529;4.204027;,
 8.016973;9.681529;6.557372;,
 5.932028;9.681529;7.879969;,
 3.484719;9.681529;7.552958;,
 1.820169;9.681529;5.729355;,
 1.717240;9.681529;3.262441;,
 3.224093;9.681529;1.306514;,
 5.494390;11.284681;1.833990;,
 7.035748;11.284681;2.640162;,
 7.698301;11.284681;4.248493;,
 7.172031;11.284681;5.906424;,
 5.703186;11.284681;6.838194;,
 3.979055;11.284681;6.607816;,
 2.806379;11.284681;5.323086;,
 2.733865;11.284681;3.585144;,
 3.795443;11.284681;2.207193;,
 5.293761;12.379650;3.335481;,
 5.916918;12.379650;3.661409;,
 6.184782;12.379650;4.311642;,
 5.972016;12.379650;4.981929;,
 5.378174;12.379650;5.358635;,
 4.681123;12.379650;5.265495;,
 4.207021;12.379650;4.746091;,
 4.177704;12.379650;4.043456;,
 4.606892;12.379650;3.486363;,
 5.135910;12.531077;4.309227;,
 -12.805912;0.000000;-6.276536;,
 12.194086;0.000000;-11.995331;,
 -9.027779;0.000000;13.004667;,
 12.194086;0.000000;7.952458;,
 -9.340024;0.000000;-11.062146;,
 -11.872727;0.000000;-8.529443;,
 -7.087118;0.000000;-11.995331;,
 -12.143047;0.000000;10.826830;,
 -10.628075;0.000000;12.341803;,
 -12.805912;0.000000;9.226534;,
 9.232703;0.000000;12.138618;,
 11.328037;0.000000;10.043284;,
 7.141877;0.000000;13.004667;,
 -9.774473;0.471154;-0.133195;,
 -8.927454;0.479309;-0.133195;,
 -9.774473;0.471154;0.611984;,
 -8.927454;0.479309;0.611984;,
 -7.968128;11.916568;-0.093911;,
 -7.435927;11.864104;-0.093911;,
 -7.968128;11.916566;0.548926;,
 -7.435927;11.864104;0.548926;,
 -4.782958;0.389078;-0.133195;,
 -5.629250;0.425076;-0.133195;,
 -6.745671;11.852754;-0.093911;,
 -6.212033;11.887690;-0.093911;,
 -5.629250;0.425076;0.611984;,
 -6.745671;11.852754;0.548926;,
 -4.782958;0.389078;0.611984;,
 -6.212033;11.887690;0.548926;,
 -8.904161;0.000000;-0.297589;,
 -5.404162;0.000000;-0.297589;,
 -8.904161;0.000000;0.702411;,
 -5.404162;0.000000;0.702411;,
 -8.904161;0.500000;-0.297589;,
 -5.404162;0.500000;-0.297589;,
 -8.904161;0.500000;0.702411;,
 -5.404162;0.500000;0.702411;,
 -10.057231;-0.095348;5.267865;,
 -4.367439;-0.095348;5.267865;,
 -10.057231;-0.095348;9.731406;,
 -4.367439;-0.095348;9.731406;,
 -9.548711;4.973999;5.888623;,
 -4.875958;4.973999;5.888623;,
 -9.548711;4.973999;8.088509;,
 -4.875958;4.973999;8.088509;,
 9.159991;0.000000;-7.766968;,
 9.159991;0.000000;7.996673;,
 -0.722186;3.300000;7.996673;,
 9.159991;3.300000;7.996673;,
 -0.722186;3.300000;-3.836969;,
 1.488439;3.300000;-7.521344;,
 0.260314;3.300000;-6.784468;,
 -0.476561;3.300000;-5.556344;,
 5.414144;9.681529;1.602901;,
 5.414144;-0.030768;1.602901;,
 5.414144;9.681529;1.602901;,
 3.039181;9.681529;0.927763;,
 1.653824;9.681529;-1.116020;,
 1.906298;9.681529;-3.572138;,
 3.678466;9.681529;-5.291347;,
 6.141112;9.681529;-5.469209;,
 8.141935;9.681529;-4.022501;,
 8.744729;9.681529;-1.628156;,
 7.667442;9.681529;0.593489;,
 5.635654;9.681529;0.776773;,
 5.635654;-0.030768;0.776773;,
 5.635654;9.681529;0.776773;,
 7.823531;9.681529;1.921091;,
 8.763986;9.681529;4.204027;,
 8.016973;9.681529;6.557372;,
 5.932028;9.681529;7.879969;,
 3.484719;9.681529;7.552958;,
 1.820169;9.681529;5.729355;,
 1.717240;9.681529;3.262441;,
 3.224093;9.681529;1.306514;,
 -9.774473;0.471154;0.611984;,
 -7.968128;11.916566;0.548926;,
 -6.212033;11.887690;-0.093911;,
 -4.782958;0.389078;-0.133195;,
 -5.404162;0.500000;-0.297589;,
 -8.904161;0.500000;-0.297589;,
 -5.404162;0.500000;0.702411;,
 -5.404162;0.500000;-0.297589;,
 -8.904161;0.500000;0.702411;,
 -5.404162;0.500000;0.702411;,
 -4.875958;4.973999;5.888623;,
 -9.548711;4.973999;5.888623;,
 -4.875958;4.973999;8.088509;,
 -4.875958;4.973999;5.888623;,
 -9.548711;4.973999;8.088509;,
 -4.875958;4.973999;8.088509;,
 9.159991;3.300000;-7.766968;,
 3.207814;3.300000;-7.766968;,
 9.159991;3.300000;7.996673;,
 9.159991;3.300000;-7.766968;,
 -0.722186;0.000000;7.996673;,
 -0.722186;3.300000;7.996673;,
 -8.927454;0.479309;-0.133195;,
 -7.435927;11.864104;-0.093911;,
 -8.927454;0.479309;0.611984;,
 -7.435927;11.864104;0.548926;,
 -7.968128;11.916568;-0.093911;,
 -9.774473;0.471154;-0.133195;,
 -5.629250;0.425076;-0.133195;,
 -6.745671;11.852754;-0.093911;,
 -5.629250;0.425076;0.611984;,
 -6.745671;11.852754;0.548926;,
 -4.782958;0.389078;0.611984;,
 -6.212033;11.887690;0.548926;,
 -8.904161;0.000000;-0.297589;,
 -5.404162;0.000000;-0.297589;,
 -5.404162;0.000000;-0.297589;,
 -5.404162;0.000000;0.702411;,
 -5.404162;0.000000;0.702411;,
 -8.904161;0.000000;0.702411;,
 -8.904161;0.000000;0.702411;,
 -8.904161;0.500000;-0.297589;,
 -8.904161;0.000000;-0.297589;,
 -8.904161;0.500000;0.702411;,
 -10.057231;-0.095348;5.267865;,
 -4.367439;-0.095348;5.267865;,
 -4.367439;-0.095348;5.267865;,
 -4.367439;-0.095348;9.731406;,
 -4.367439;-0.095348;9.731406;,
 -10.057231;-0.095348;9.731406;,
 -10.057231;-0.095348;9.731406;,
 -9.548711;4.973999;5.888623;,
 -10.057231;-0.095348;5.267865;,
 -9.548711;4.973999;8.088509;,
 -7.711640;10.203489;-5.185626;,
 -6.472373;10.203489;-5.185626;,
 -7.711640;13.090404;7.318978;,
 -6.472373;13.090404;7.318978;,
 -7.711640;11.873905;-5.604883;,
 -6.472373;11.873905;-5.604883;,
 -7.711640;14.760819;6.899720;,
 -6.472373;14.760819;6.899720;,
 -8.053741;6.336339;-4.511158;,
 -6.185956;6.336339;-4.511158;,
 -8.053741;14.791324;-6.161093;,
 -6.185956;14.791324;-6.161093;,
 -8.053741;6.447457;-4.958660;,
 -6.185956;6.447457;-4.958660;,
 -8.053741;14.659976;-6.730028;,
 -6.185956;14.659976;-6.730028;,
 -8.053741;10.416142;-5.151001;,
 -6.185956;10.416142;-5.151001;,
 -6.185956;9.853524;-6.474610;,
 -8.053741;9.853524;-6.474610;,
 -6.185956;12.693417;-6.985348;,
 -8.053741;12.693417;-6.985348;,
 -6.472373;14.760819;6.899720;,
 -6.472373;11.873905;-5.604883;,
 -6.472373;13.090404;7.318978;,
 -7.711640;13.090404;7.318978;,
 -8.053741;6.336339;-4.511158;,
 -6.185956;6.336339;-4.511158;,
 -6.185956;9.853524;-6.474610;,
 -6.185956;6.447457;-4.958660;,
 -6.185956;14.791324;-6.161093;,
 -6.185956;14.659976;-6.730028;,
 -8.053741;6.447457;-4.958660;,
 -8.053741;14.791324;-6.161093;,
 -8.053741;9.853524;-6.474610;,
 -8.053741;14.791324;-6.161093;,
 -6.185956;14.791324;-6.161093;,
 -6.185956;12.693417;-6.985348;,
 -6.472373;10.203489;-5.185626;,
 -6.472373;13.090404;7.318978;,
 -7.711640;14.760819;6.899720;,
 -6.472373;14.760819;6.899720;,
 -7.711640;13.090404;7.318978;,
 -7.711640;11.873905;-5.604883;,
 -7.711640;10.203489;-5.185626;,
 -7.711640;14.760819;6.899720;,
 -6.185956;6.447457;-4.958660;,
 -8.053741;6.447457;-4.958660;,
 -6.185956;10.416142;-5.151001;,
 -6.185956;6.336339;-4.511158;,
 -8.053741;10.416142;-5.151001;,
 -8.053741;6.336339;-4.511158;,
 -8.053741;14.659976;-6.730028;,
 -6.185956;10.416142;-5.151001;,
 -8.053741;10.416142;-5.151001;,
 -6.185956;14.659976;-6.730028;,
 -8.053741;12.693417;-6.985348;,
 -8.053741;14.659976;-6.730028;,
 -6.185956;9.853524;-6.474610;,
 -8.053741;12.693417;-6.985348;,
 -6.185956;12.693417;-6.985348;,
 -8.053741;9.853524;-6.474610;,
 -8.244046;4.973999;6.839675;,
 -5.884755;4.973999;6.839675;,
 -8.244046;4.973999;7.392598;,
 -5.884755;4.973999;7.392598;,
 -7.706151;13.141847;6.839675;,
 -6.428476;13.141847;6.839675;,
 -7.706151;13.141847;7.392598;,
 -6.428476;13.141847;7.392598;,
 -5.884755;4.973999;6.839675;,
 -5.884755;4.973999;7.392598;,
 -8.244046;4.973999;7.392598;,
 -8.244046;4.973999;6.839675;,
 -6.428476;13.141847;6.839675;,
 -7.706151;13.141847;6.839675;,
 -6.428476;13.141847;7.392598;,
 -6.428476;13.141847;6.839675;,
 -7.706151;13.141847;7.392598;,
 -6.428476;13.141847;7.392598;,
 -7.706151;13.141847;6.839675;,
 -7.706151;13.141847;7.392598;;
 243;
 3;4,7,5;,
 3;4,6,7;,
 3;15,6,4;,
 3;14,6,15;,
 3;13,6,14;,
 3;9,6,13;,
 3;0,181,1;,
 3;181,0,182;,
 3;135,183,3;,
 3;183,135,184;,
 3;136,137,2;,
 3;137,136,138;,
 3;185,139,8;,
 3;139,185,186;,
 3;10,182,0;,
 3;182,10,140;,
 3;11,140,10;,
 3;140,11,141;,
 3;12,141,11;,
 3;141,12,142;,
 3;8,142,12;,
 3;142,8,139;,
 3;16,26,17;,
 3;26,16,25;,
 3;17,27,18;,
 3;27,17,26;,
 3;18,28,19;,
 3;28,18,27;,
 3;19,29,20;,
 3;29,19,28;,
 3;20,30,21;,
 3;30,20,29;,
 3;21,31,22;,
 3;31,21,30;,
 3;22,32,23;,
 3;32,22,31;,
 3;23,33,24;,
 3;33,23,32;,
 3;24,143,144;,
 3;143,24,33;,
 3;145,35,146;,
 3;35,145,34;,
 3;146,36,147;,
 3;36,146,35;,
 3;147,37,148;,
 3;37,147,36;,
 3;148,38,149;,
 3;38,148,37;,
 3;149,39,150;,
 3;39,149,38;,
 3;150,40,151;,
 3;40,150,39;,
 3;151,41,152;,
 3;41,151,40;,
 3;152,42,153;,
 3;42,152,41;,
 3;153,34,145;,
 3;34,153,42;,
 3;34,44,35;,
 3;44,34,43;,
 3;35,45,36;,
 3;45,35,44;,
 3;36,46,37;,
 3;46,36,45;,
 3;37,47,38;,
 3;47,37,46;,
 3;38,48,39;,
 3;48,38,47;,
 3;39,49,40;,
 3;49,39,48;,
 3;40,50,41;,
 3;50,40,49;,
 3;41,51,42;,
 3;51,41,50;,
 3;42,43,34;,
 3;43,42,51;,
 3;44,52,45;,
 3;45,52,46;,
 3;46,52,47;,
 3;47,52,48;,
 3;48,52,49;,
 3;49,52,50;,
 3;50,52,51;,
 3;51,52,43;,
 3;43,52,44;,
 3;53,63,54;,
 3;63,53,62;,
 3;54,64,55;,
 3;64,54,63;,
 3;55,65,56;,
 3;65,55,64;,
 3;56,66,57;,
 3;66,56,65;,
 3;57,67,58;,
 3;67,57,66;,
 3;58,68,59;,
 3;68,58,67;,
 3;59,69,60;,
 3;69,59,68;,
 3;60,70,61;,
 3;70,60,69;,
 3;61,154,155;,
 3;154,61,70;,
 3;156,72,157;,
 3;72,156,71;,
 3;157,73,158;,
 3;73,157,72;,
 3;158,74,159;,
 3;74,158,73;,
 3;159,75,160;,
 3;75,159,74;,
 3;160,76,161;,
 3;76,160,75;,
 3;161,77,162;,
 3;77,161,76;,
 3;162,78,163;,
 3;78,162,77;,
 3;163,79,164;,
 3;79,163,78;,
 3;164,71,156;,
 3;71,164,79;,
 3;71,81,72;,
 3;81,71,80;,
 3;72,82,73;,
 3;82,72,81;,
 3;73,83,74;,
 3;83,73,82;,
 3;74,84,75;,
 3;84,74,83;,
 3;75,85,76;,
 3;85,75,84;,
 3;76,86,77;,
 3;86,76,85;,
 3;77,87,78;,
 3;87,77,86;,
 3;78,88,79;,
 3;88,78,87;,
 3;79,80,71;,
 3;80,79,88;,
 3;81,89,82;,
 3;82,89,83;,
 3;83,89,84;,
 3;84,89,85;,
 3;85,89,86;,
 3;86,89,87;,
 3;87,89,88;,
 3;88,89,80;,
 3;80,89,81;,
 3;98,99,97;,
 3;99,95,90;,
 3;95,96,94;,
 3;99,96,95;,
 3;91,101,93;,
 3;101,102,100;,
 3;91,102,101;,
 3;96,102,91;,
 3;99,102,96;,
 3;98,102,99;,
 3;92,102,98;,
 3;103,108,104;,
 3;108,103,107;,
 3;187,110,106;,
 3;110,187,188;,
 3;189,109,105;,
 3;109,189,190;,
 3;165,191,192;,
 3;191,165,166;,
 3;111,113,114;,
 3;113,111,112;,
 3;193,116,194;,
 3;116,193,115;,
 3;195,118,196;,
 3;118,195,117;,
 3;197,167,198;,
 3;167,197,168;,
 3;119,122,121;,
 3;122,119,120;,
 3;123,126,124;,
 3;126,123,125;,
 3;199,169,200;,
 3;169,199,170;,
 3;201,171,202;,
 3;171,201,172;,
 3;203,173,204;,
 3;173,203,174;,
 3;205,206,207;,
 3;206,205,208;,
 3;127,130,129;,
 3;130,127,128;,
 3;131,134,132;,
 3;134,131,133;,
 3;209,175,210;,
 3;175,209,176;,
 3;211,177,212;,
 3;177,211,178;,
 3;213,179,214;,
 3;179,213,180;,
 3;215,216,217;,
 3;216,215,218;,
 3;219,222,221;,
 3;222,219,220;,
 3;223,226,224;,
 3;226,223,225;,
 3;257,241,258;,
 3;241,257,242;,
 3;243,259,244;,
 3;259,243,260;,
 3;261,262,263;,
 3;262,261,264;,
 3;235,228,236;,
 3;228,235,227;,
 3;232,238,237;,
 3;238,232,231;,
 3;245,265,246;,
 3;265,245,266;,
 3;267,234,230;,
 3;268,234,267;,
 3;234,247,239;,
 3;268,247,234;,
 3;268,248,247;,
 3;249,233,229;,
 3;233,249,250;,
 3;269,251,270;,
 3;252,251,269;,
 3;251,240,253;,
 3;252,240,251;,
 3;252,271,240;,
 3;254,272,255;,
 3;272,254,273;,
 3;274,275,276;,
 3;275,274,256;,
 3;277,278,279;,
 3;278,277,280;,
 3;285,288,286;,
 3;288,285,287;,
 3;281,293,282;,
 3;293,281,294;,
 3;289,295,284;,
 3;295,289,296;,
 3;290,297,283;,
 3;297,290,298;,
 3;291,299,292;,
 3;299,291,300;;

 MeshNormals {
  301;
  -0.070889;0.000000;-0.997484;,
  0.000000;0.000000;-1.000000;,
  0.000000;0.000000;1.000000;,
  1.000000;0.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  -0.997484;0.000000;-0.070889;,
  0.000000;1.000000;0.000000;,
  -0.334579;0.000000;-0.942368;,
  -0.707107;0.000000;-0.707107;,
  -0.942368;0.000000;-0.334579;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.072037;0.000000;0.997402;,
  -0.585934;0.000000;0.810359;,
  -0.969740;0.000000;0.244139;,
  -0.899794;0.000000;-0.436315;,
  -0.408824;0.000000;-0.912613;,
  0.273439;0.000000;-0.961889;,
  0.827757;0.000000;-0.561086;,
  0.994758;0.000000;0.102255;,
  0.696301;0.000000;0.717750;,
  0.072037;0.000000;0.997402;,
  -0.585934;0.000000;0.810358;,
  -0.969740;0.000000;0.244139;,
  -0.899794;0.000000;-0.436315;,
  -0.408824;0.000000;-0.912613;,
  0.273439;0.000000;-0.961889;,
  0.827757;0.000000;-0.561086;,
  0.994758;0.000000;0.102255;,
  0.696301;0.000000;0.717750;,
  0.053198;0.674267;0.736569;,
  -0.432705;0.674267;0.598440;,
  -0.716141;0.674267;0.180294;,
  -0.664487;0.674267;-0.322214;,
  -0.301911;0.674267;-0.673954;,
  0.201932;0.674267;-0.710344;,
  0.611289;0.674267;-0.414356;,
  0.734617;0.674267;0.075514;,
  0.514210;0.674267;0.530050;,
  0.030119;0.906556;0.421009;,
  -0.246838;0.907240;0.340569;,
  -0.406331;0.908031;0.101855;,
  -0.375783;0.908571;-0.182443;,
  -0.170795;0.908656;-0.381016;,
  0.114058;0.908257;-0.402567;,
  0.347398;0.907517;-0.236067;,
  0.419496;0.906750;0.042743;,
  0.294248;0.906352;0.303222;,
  0.004646;0.999971;0.006090;,
  0.132443;0.000000;-0.991190;,
  0.738582;0.000000;-0.674164;,
  0.999131;0.000000;-0.041688;,
  0.792175;0.000000;0.610294;,
  0.214552;0.000000;0.976712;,
  -0.463463;0.000000;0.886116;,
  -0.924618;0.000000;0.380896;,
  -0.953134;0.000000;-0.302549;,
  -0.535668;0.000000;-0.844429;,
  0.132443;0.000000;-0.991190;,
  0.738582;0.000000;-0.674163;,
  0.999131;0.000000;-0.041687;,
  0.792175;0.000000;0.610294;,
  0.214552;0.000000;0.976712;,
  -0.463463;0.000000;0.886116;,
  -0.924618;0.000000;0.380896;,
  -0.953134;0.000000;-0.302549;,
  -0.535668;0.000000;-0.844429;,
  0.097807;0.674267;-0.731982;,
  0.545434;0.674267;-0.497862;,
  0.737846;0.674267;-0.030786;,
  0.585012;0.674267;0.450695;,
  0.158444;0.674267;0.721290;,
  -0.342262;0.674267;0.654386;,
  -0.682819;0.674267;0.281287;,
  -0.703878;0.674267;-0.223429;,
  -0.395584;0.674267;-0.623600;,
  0.056187;0.906556;-0.418329;,
  0.310979;0.907240;-0.283211;,
  0.418556;0.908031;-0.017035;,
  0.330793;0.908571;0.255097;,
  0.089684;0.908656;0.407800;,
  -0.193594;0.908257;0.370933;,
  -0.388169;0.907517;0.160431;,
  -0.402020;0.906750;-0.127216;,
  -0.226385;0.906352;-0.356757;,
  -0.003310;0.999971;-0.006908;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  -0.000021;0.003452;-0.999994;,
  -0.000033;0.003455;-0.999994;,
  -0.000053;0.005518;0.999985;,
  0.991527;-0.129900;0.000000;,
  0.000333;0.003380;-0.999994;,
  0.000316;0.003383;-0.999994;,
  0.000513;0.005470;0.999985;,
  0.991527;-0.129900;0.000000;,
  0.000135;0.003450;-0.999994;,
  0.000147;0.003452;-0.999994;,
  -0.000205;0.003392;-0.999994;,
  -0.000222;0.003389;-0.999994;,
  -0.995262;-0.097232;0.000000;,
  -0.995262;-0.097232;0.000000;,
  0.000235;0.005513;0.999985;,
  -0.000332;0.005484;0.999985;,
  0.000000;-1.000000;0.000000;,
  0.000000;-1.000000;0.000000;,
  0.000000;-1.000000;0.000000;,
  0.000000;-1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;-1.000000;0.000000;,
  0.000000;-1.000000;0.000000;,
  0.000000;-1.000000;0.000000;,
  0.000000;-1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  0.000000;0.000000;1.000000;,
  0.000000;0.000000;1.000000;,
  0.000000;0.000000;1.000000;,
  -0.997484;0.000000;-0.070889;,
  -0.334579;0.000000;-0.942368;,
  -0.707107;0.000000;-0.707107;,
  -0.942368;0.000000;-0.334579;,
  0.072037;0.000000;0.997402;,
  0.072037;0.000000;0.997402;,
  0.059975;0.553925;0.830403;,
  -0.487829;0.553925;0.674677;,
  -0.807373;0.553925;0.203262;,
  -0.749138;0.553925;-0.363262;,
  -0.340373;0.553925;-0.759811;,
  0.227656;0.553925;-0.800837;,
  0.689163;0.553925;-0.467142;,
  0.828202;0.553925;0.085134;,
  0.579717;0.553925;0.597574;,
  0.132443;0.000000;-0.991190;,
  0.132443;0.000000;-0.991190;,
  0.110268;0.553925;-0.825232;,
  0.614919;0.553925;-0.561286;,
  0.831843;0.553925;-0.034708;,
  0.659538;0.553925;0.508111;,
  0.178628;0.553925;0.813178;,
  -0.385864;0.553925;0.737751;,
  -0.769806;0.553925;0.317121;,
  -0.793547;0.553925;-0.251892;,
  -0.445979;0.553925;-0.703043;,
  -0.987774;0.155893;0.000000;,
  -0.987774;0.155893;0.000000;,
  0.992365;0.123334;0.000000;,
  0.992365;0.123334;0.000000;,
  0.000000;0.000000;-1.000000;,
  0.000000;0.000000;-1.000000;,
  1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  0.000000;0.000000;1.000000;,
  0.000000;0.000000;1.000000;,
  0.000000;0.121545;-0.992586;,
  0.000000;0.121545;-0.992586;,
  0.995006;0.099812;0.000000;,
  0.995006;0.099812;0.000000;,
  0.000000;0.308298;0.951290;,
  0.000000;0.308298;0.951290;,
  0.000000;0.000000;-1.000000;,
  -0.070889;0.000000;-0.997484;,
  1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  0.991527;-0.129900;0.000000;,
  0.991527;-0.129900;0.000000;,
  -0.000037;0.005516;0.999985;,
  0.000539;0.005468;0.999985;,
  -0.987774;0.155893;0.000000;,
  -0.987774;0.155893;0.000000;,
  -0.995262;-0.097232;0.000000;,
  -0.995262;-0.097232;0.000000;,
  0.000218;0.005512;0.999985;,
  -0.000359;0.005483;0.999985;,
  0.992365;0.123334;0.000000;,
  0.992365;0.123334;0.000000;,
  0.000000;0.000000;-1.000000;,
  0.000000;0.000000;-1.000000;,
  1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  0.000000;0.000000;1.000000;,
  0.000000;0.000000;1.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  0.000000;0.121545;-0.992586;,
  0.000000;0.121545;-0.992586;,
  0.995006;0.099812;0.000000;,
  0.995006;0.099812;0.000000;,
  0.000000;0.308298;0.951290;,
  0.000000;0.308298;0.951290;,
  -0.995006;0.099812;0.000000;,
  -0.995006;0.099812;0.000000;,
  -0.995006;0.099812;0.000000;,
  -0.995006;0.099812;0.000000;,
  0.000000;-0.974370;0.224951;,
  0.000000;-0.974370;0.224951;,
  0.000000;-0.974370;0.224951;,
  0.000000;-0.974370;0.224951;,
  0.000000;0.974370;-0.224951;,
  0.000000;0.974370;-0.224951;,
  0.000000;0.974370;-0.224951;,
  0.000000;0.974370;-0.224951;,
  0.000000;0.078032;0.996951;,
  0.000000;0.078032;0.996951;,
  0.000000;0.974370;-0.224951;,
  1.000000;0.000000;0.000000;,
  0.000000;-0.576012;-0.817442;,
  0.000000;-0.576012;-0.817442;,
  0.000000;0.974370;-0.224951;,
  1.000000;0.000000;0.000000;,
  0.000000;0.078032;0.996951;,
  0.000000;0.078032;0.996951;,
  0.000000;-0.576012;-0.817442;,
  0.000000;-0.576012;-0.817442;,
  1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  0.000000;0.263618;0.964627;,
  0.000000;0.263618;0.964627;,
  0.000000;-0.999941;-0.010831;,
  0.000000;-0.999941;-0.010831;,
  1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  0.000000;0.974370;-0.224951;,
  0.000000;0.974370;-0.224951;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  0.000000;0.224951;0.974370;,
  0.000000;0.224951;0.974370;,
  0.000000;0.421930;-0.906628;,
  1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  0.000000;0.263618;0.964627;,
  0.000000;0.263618;0.964627;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  0.000000;-0.999941;-0.010831;,
  0.000000;-0.999941;-0.010831;,
  1.000000;0.000000;0.000000;,
  1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  -1.000000;0.000000;0.000000;,
  0.000000;0.224951;0.974370;,
  0.000000;0.224951;0.974370;,
  0.000000;0.421930;-0.906628;,
  0.000000;0.421930;-0.906628;,
  0.000000;0.421930;-0.906628;,
  0.000000;-0.124158;-0.992262;,
  0.000000;-0.124158;-0.992262;,
  0.000000;-0.124158;-0.992262;,
  0.000000;-0.124158;-0.992262;,
  0.000000;0.000000;-1.000000;,
  0.000000;0.000000;-1.000000;,
  0.000000;0.000000;1.000000;,
  0.994434;0.105357;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.000000;1.000000;0.000000;,
  0.994434;0.105357;0.000000;,
  0.000000;0.000000;1.000000;,
  -0.994552;0.104240;0.000000;,
  -0.994552;0.104240;0.000000;,
  0.000000;0.000000;-1.000000;,
  0.000000;0.000000;-1.000000;,
  0.994434;0.105357;0.000000;,
  0.994434;0.105357;0.000000;,
  0.000000;0.000000;1.000000;,
  0.000000;0.000000;1.000000;,
  -0.994552;0.104240;0.000000;,
  -0.994552;0.104240;0.000000;;
  243;
  3;4,7,5;,
  3;4,6,7;,
  3;15,6,4;,
  3;14,6,15;,
  3;13,6,14;,
  3;9,6,13;,
  3;0,181,1;,
  3;181,0,182;,
  3;135,183,3;,
  3;183,135,184;,
  3;136,137,2;,
  3;137,136,138;,
  3;185,139,8;,
  3;139,185,186;,
  3;10,182,0;,
  3;182,10,140;,
  3;11,140,10;,
  3;140,11,141;,
  3;12,141,11;,
  3;141,12,142;,
  3;8,142,12;,
  3;142,8,139;,
  3;16,26,17;,
  3;26,16,25;,
  3;17,27,18;,
  3;27,17,26;,
  3;18,28,19;,
  3;28,18,27;,
  3;19,29,20;,
  3;29,19,28;,
  3;20,30,21;,
  3;30,20,29;,
  3;21,31,22;,
  3;31,21,30;,
  3;22,32,23;,
  3;32,22,31;,
  3;23,33,24;,
  3;33,23,32;,
  3;24,143,144;,
  3;143,24,33;,
  3;145,35,146;,
  3;35,145,34;,
  3;146,36,147;,
  3;36,146,35;,
  3;147,37,148;,
  3;37,147,36;,
  3;148,38,149;,
  3;38,148,37;,
  3;149,39,150;,
  3;39,149,38;,
  3;150,40,151;,
  3;40,150,39;,
  3;151,41,152;,
  3;41,151,40;,
  3;152,42,153;,
  3;42,152,41;,
  3;153,34,145;,
  3;34,153,42;,
  3;34,44,35;,
  3;44,34,43;,
  3;35,45,36;,
  3;45,35,44;,
  3;36,46,37;,
  3;46,36,45;,
  3;37,47,38;,
  3;47,37,46;,
  3;38,48,39;,
  3;48,38,47;,
  3;39,49,40;,
  3;49,39,48;,
  3;40,50,41;,
  3;50,40,49;,
  3;41,51,42;,
  3;51,41,50;,
  3;42,43,34;,
  3;43,42,51;,
  3;44,52,45;,
  3;45,52,46;,
  3;46,52,47;,
  3;47,52,48;,
  3;48,52,49;,
  3;49,52,50;,
  3;50,52,51;,
  3;51,52,43;,
  3;43,52,44;,
  3;53,63,54;,
  3;63,53,62;,
  3;54,64,55;,
  3;64,54,63;,
  3;55,65,56;,
  3;65,55,64;,
  3;56,66,57;,
  3;66,56,65;,
  3;57,67,58;,
  3;67,57,66;,
  3;58,68,59;,
  3;68,58,67;,
  3;59,69,60;,
  3;69,59,68;,
  3;60,70,61;,
  3;70,60,69;,
  3;61,154,155;,
  3;154,61,70;,
  3;156,72,157;,
  3;72,156,71;,
  3;157,73,158;,
  3;73,157,72;,
  3;158,74,159;,
  3;74,158,73;,
  3;159,75,160;,
  3;75,159,74;,
  3;160,76,161;,
  3;76,160,75;,
  3;161,77,162;,
  3;77,161,76;,
  3;162,78,163;,
  3;78,162,77;,
  3;163,79,164;,
  3;79,163,78;,
  3;164,71,156;,
  3;71,164,79;,
  3;71,81,72;,
  3;81,71,80;,
  3;72,82,73;,
  3;82,72,81;,
  3;73,83,74;,
  3;83,73,82;,
  3;74,84,75;,
  3;84,74,83;,
  3;75,85,76;,
  3;85,75,84;,
  3;76,86,77;,
  3;86,76,85;,
  3;77,87,78;,
  3;87,77,86;,
  3;78,88,79;,
  3;88,78,87;,
  3;79,80,71;,
  3;80,79,88;,
  3;81,89,82;,
  3;82,89,83;,
  3;83,89,84;,
  3;84,89,85;,
  3;85,89,86;,
  3;86,89,87;,
  3;87,89,88;,
  3;88,89,80;,
  3;80,89,81;,
  3;98,99,97;,
  3;99,95,90;,
  3;95,96,94;,
  3;99,96,95;,
  3;91,101,93;,
  3;101,102,100;,
  3;91,102,101;,
  3;96,102,91;,
  3;99,102,96;,
  3;98,102,99;,
  3;92,102,98;,
  3;103,108,104;,
  3;108,103,107;,
  3;187,110,106;,
  3;110,187,188;,
  3;189,109,105;,
  3;109,189,190;,
  3;165,191,192;,
  3;191,165,166;,
  3;111,113,114;,
  3;113,111,112;,
  3;193,116,194;,
  3;116,193,115;,
  3;195,118,196;,
  3;118,195,117;,
  3;197,167,198;,
  3;167,197,168;,
  3;119,122,121;,
  3;122,119,120;,
  3;123,126,124;,
  3;126,123,125;,
  3;199,169,200;,
  3;169,199,170;,
  3;201,171,202;,
  3;171,201,172;,
  3;203,173,204;,
  3;173,203,174;,
  3;205,206,207;,
  3;206,205,208;,
  3;127,130,129;,
  3;130,127,128;,
  3;131,134,132;,
  3;134,131,133;,
  3;209,175,210;,
  3;175,209,176;,
  3;211,177,212;,
  3;177,211,178;,
  3;213,179,214;,
  3;179,213,180;,
  3;215,216,217;,
  3;216,215,218;,
  3;219,222,221;,
  3;222,219,220;,
  3;223,226,224;,
  3;226,223,225;,
  3;257,241,258;,
  3;241,257,242;,
  3;243,259,244;,
  3;259,243,260;,
  3;261,262,263;,
  3;262,261,264;,
  3;235,228,236;,
  3;228,235,227;,
  3;232,238,237;,
  3;238,232,231;,
  3;245,265,246;,
  3;265,245,266;,
  3;267,234,230;,
  3;268,234,267;,
  3;234,247,239;,
  3;268,247,234;,
  3;268,248,247;,
  3;249,233,229;,
  3;233,249,250;,
  3;269,251,270;,
  3;252,251,269;,
  3;251,240,253;,
  3;252,240,251;,
  3;252,271,240;,
  3;254,272,255;,
  3;272,254,273;,
  3;274,275,276;,
  3;275,274,256;,
  3;277,278,279;,
  3;278,277,280;,
  3;285,288,286;,
  3;288,285,287;,
  3;281,293,282;,
  3;293,281,294;,
  3;289,295,284;,
  3;295,289,296;,
  3;290,297,283;,
  3;297,290,298;,
  3;291,299,292;,
  3;299,291,300;;
 }

 MeshTextureCoords {
  301;
  0.815319;0.986268;,
  0.929283;0.986268;,
  0.467391;0.986268;,
  0.992466;0.621264;,
  0.815319;0.924899;,
  0.929283;0.924899;,
  0.740072;0.623078;,
  0.929283;0.623078;,
  0.693965;0.986268;,
  0.740072;0.849653;,
  0.782064;0.986268;,
  0.754642;0.986268;,
  0.727219;0.986268;,
  0.744775;0.882573;,
  0.758884;0.906087;,
  0.782398;0.920196;,
  0.742339;0.517277;,
  0.770478;0.517277;,
  0.798741;0.517277;,
  0.826589;0.517277;,
  0.853539;0.517277;,
  0.880019;0.517277;,
  0.906969;0.517277;,
  0.934817;0.517277;,
  0.963080;0.517277;,
  0.742339;0.268397;,
  0.770478;0.268397;,
  0.798741;0.268397;,
  0.826589;0.268397;,
  0.853539;0.268397;,
  0.880019;0.268397;,
  0.906969;0.268397;,
  0.934817;0.268397;,
  0.963080;0.268397;,
  0.571717;0.150426;,
  0.552211;0.096834;,
  0.502821;0.068319;,
  0.446656;0.078222;,
  0.409997;0.121910;,
  0.409997;0.178942;,
  0.446656;0.222630;,
  0.502821;0.232534;,
  0.552211;0.204018;,
  0.522050;0.150426;,
  0.514165;0.128759;,
  0.494196;0.117231;,
  0.471489;0.121235;,
  0.456669;0.138897;,
  0.456669;0.161955;,
  0.471489;0.179617;,
  0.494196;0.183621;,
  0.514165;0.172093;,
  0.489720;0.151327;,
  0.743460;0.256631;,
  0.771158;0.256631;,
  0.798977;0.256631;,
  0.826388;0.256631;,
  0.852915;0.256631;,
  0.878981;0.256631;,
  0.905508;0.256631;,
  0.932919;0.256631;,
  0.960738;0.256631;,
  0.743460;0.011655;,
  0.771158;0.011655;,
  0.798977;0.011655;,
  0.826388;0.011655;,
  0.852915;0.011655;,
  0.878981;0.011655;,
  0.905508;0.011655;,
  0.932919;0.011655;,
  0.960738;0.011655;,
  0.567071;0.405926;,
  0.547404;0.351892;,
  0.497605;0.323140;,
  0.440976;0.333126;,
  0.404014;0.377175;,
  0.404014;0.434678;,
  0.440976;0.478727;,
  0.497605;0.488712;,
  0.547404;0.459961;,
  0.516994;0.405926;,
  0.509042;0.384081;,
  0.488909;0.372457;,
  0.466015;0.376494;,
  0.451071;0.394302;,
  0.451071;0.417550;,
  0.466015;0.435359;,
  0.488909;0.439396;,
  0.509042;0.427772;,
  0.484396;0.406835;,
  0.017848;0.263876;,
  0.338063;0.337126;,
  0.066241;0.016911;,
  0.338063;0.081622;,
  0.062241;0.325173;,
  0.029801;0.292733;,
  0.091098;0.337126;,
  0.026339;0.044806;,
  0.045743;0.025401;,
  0.017848;0.065303;,
  0.300132;0.028004;,
  0.326971;0.054842;,
  0.273352;0.016911;,
  0.348729;0.914898;,
  0.414151;0.913765;,
  0.542576;0.914918;,
  0.477155;0.913038;,
  0.350192;0.657003;,
  0.414255;0.656372;,
  0.543113;0.657020;,
  0.476528;0.657651;,
  0.272435;0.986766;,
  0.204788;0.986904;,
  0.206413;0.723432;,
  0.272363;0.722455;,
  0.139619;0.989071;,
  0.140307;0.727879;,
  0.075014;0.989694;,
  0.075118;0.726901;,
  0.687702;0.627925;,
  0.687702;0.877925;,
  0.616273;0.627925;,
  0.616273;0.877925;,
  0.687702;0.592211;,
  0.687702;0.342211;,
  0.616273;0.592211;,
  0.616273;0.342211;,
  0.272738;0.598102;,
  0.146418;0.598102;,
  0.272738;0.549262;,
  0.146418;0.549262;,
  0.385284;0.598102;,
  0.511605;0.598102;,
  0.385284;0.549262;,
  0.511605;0.549262;,
  0.992466;0.923085;,
  0.278181;0.986268;,
  0.467391;0.923085;,
  0.278181;0.923085;,
  0.693965;0.923085;,
  0.782064;0.923085;,
  0.754642;0.923085;,
  0.727219;0.923085;,
  0.991219;0.268397;,
  0.991219;0.517277;,
  0.606688;0.150426;,
  0.579001;0.074355;,
  0.508893;0.033879;,
  0.429171;0.047936;,
  0.377135;0.109950;,
  0.377135;0.190902;,
  0.429171;0.252916;,
  0.508893;0.266973;,
  0.579001;0.226497;,
  0.988436;0.011655;,
  0.988436;0.256631;,
  0.602331;0.405926;,
  0.574414;0.329227;,
  0.503728;0.288416;,
  0.423346;0.302590;,
  0.370881;0.365116;,
  0.370881;0.446737;,
  0.423346;0.509263;,
  0.503728;0.523437;,
  0.574414;0.482626;,
  0.285725;0.912679;,
  0.283023;0.655729;,
  0.007491;0.727647;,
  0.009084;0.991197;,
  0.723416;0.877925;,
  0.723416;0.627925;,
  0.616273;0.913640;,
  0.687702;0.913640;,
  0.580559;0.627925;,
  0.580559;0.877925;,
  0.146418;0.710648;,
  0.272738;0.710648;,
  0.033872;0.549262;,
  0.033872;0.598102;,
  0.272738;0.436716;,
  0.146418;0.436716;,
  0.929283;0.924899;,
  0.815319;0.924899;,
  0.929283;0.623078;,
  0.929283;0.924899;,
  0.467391;0.986268;,
  0.467391;0.923085;,
  0.414151;0.913765;,
  0.414255;0.656372;,
  0.477155;0.913038;,
  0.476528;0.657651;,
  0.350192;0.657003;,
  0.348729;0.914898;,
  0.204788;0.986904;,
  0.206413;0.723432;,
  0.139619;0.989071;,
  0.140307;0.727879;,
  0.075014;0.989694;,
  0.075118;0.726901;,
  0.687702;0.627925;,
  0.687702;0.877925;,
  0.687702;0.877925;,
  0.616273;0.877925;,
  0.616273;0.877925;,
  0.616273;0.627925;,
  0.616273;0.627925;,
  0.687702;0.592211;,
  0.687702;0.627925;,
  0.616273;0.592211;,
  0.272738;0.598102;,
  0.146418;0.598102;,
  0.146418;0.598102;,
  0.146418;0.549262;,
  0.146418;0.549262;,
  0.272738;0.549262;,
  0.272738;0.549262;,
  0.385284;0.598102;,
  0.272738;0.598102;,
  0.385284;0.549262;,
  0.160074;0.017745;,
  0.218949;0.017745;,
  0.160074;0.900822;,
  0.218949;0.900822;,
  0.078269;0.015492;,
  0.019394;0.015492;,
  0.078269;0.898568;,
  0.019394;0.898568;,
  0.502115;0.974473;,
  0.429902;0.974473;,
  0.540051;0.614815;,
  0.412031;0.641680;,
  0.682182;0.959693;,
  0.748948;0.932177;,
  0.552510;0.645046;,
  0.379507;0.645046;,
  0.502115;0.814364;,
  0.429902;0.814364;,
  0.692940;0.796275;,
  0.626174;0.823791;,
  0.348407;0.720512;,
  0.583610;0.720512;,
  0.300754;0.898568;,
  0.300754;0.015492;,
  0.019394;0.980404;,
  0.078269;0.980404;,
  0.691476;0.982247;,
  0.758243;0.954731;,
  0.352012;0.832160;,
  0.406175;0.968807;,
  0.606817;0.587300;,
  0.619276;0.617531;,
  0.525842;0.968807;,
  0.519986;0.641680;,
  0.580005;0.832160;,
  0.502115;0.640758;,
  0.429902;0.640758;,
  0.650377;0.692996;,
  0.218949;0.017745;,
  0.218949;0.900822;,
  0.078269;0.898568;,
  0.019394;0.898568;,
  0.160074;0.900822;,
  0.078269;0.015492;,
  0.160074;0.017745;,
  0.078269;0.898568;,
  0.748948;0.932177;,
  0.682182;0.959693;,
  0.429902;0.814364;,
  0.429902;0.974473;,
  0.502115;0.814364;,
  0.502115;0.974473;,
  0.552510;0.645046;,
  0.429902;0.814364;,
  0.502115;0.814364;,
  0.619276;0.617531;,
  0.583610;0.720512;,
  0.552510;0.645046;,
  0.692940;0.796275;,
  0.583610;0.720512;,
  0.650377;0.692996;,
  0.626174;0.823791;,
  0.652568;0.577995;,
  0.746496;0.577995;,
  0.652568;0.030592;,
  0.983517;0.288352;,
  0.673983;0.320235;,
  0.724850;0.320235;,
  0.673983;0.288352;,
  0.724850;0.288352;,
  0.983517;0.320235;,
  0.746496;0.030592;,
  0.415335;0.288352;,
  0.415335;0.320235;,
  0.724850;0.320235;,
  0.673983;0.320235;,
  0.724850;0.288352;,
  0.724850;0.320235;,
  0.673983;0.288352;,
  0.724850;0.288352;,
  0.673983;0.320235;,
  0.673983;0.288352;;
 }

 MeshMaterialList {
  3;
  243;
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  2,
  2,
  2,
  2,
  2,
  2,
  2,
  2,
  2,
  2;

  Material {
   1.000000;1.000000;1.000000;1.000000;;
   3.200000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;

   TextureFilename {
    "oilderrick.dds";
   }
  }

  Material {
   1.000000;1.000000;1.000000;1.000000;;
   3.200000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;

   TextureFilename {
    "oilderrick2.dds";
   }
  }

  Material {
   1.000000;1.000000;1.000000;1.000000;;
   3.200000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;

   TextureFilename {
    "oilderrick2.dds";
   }
  }
 }
}