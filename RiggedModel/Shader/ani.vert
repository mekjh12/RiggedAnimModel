﻿#version 420 core

const int MAX_JOINTS = 128;
const int MAX_WEIGHTS = 3;

layout(location = 0) in vec3 in_position;
layout(location = 1) in vec2 in_textureCoords;
layout(location = 2) in vec3 in_normal;
layout(location = 3) in vec4 in_jointIndices;
layout(location = 4) in vec4 in_weights;

out vec3 pass_normals;
out vec2 pass_textureCoords;
out vec4 pass_weights;

uniform mat4 jointTransforms[MAX_JOINTS];
uniform mat4 proj;
uniform mat4 view;
uniform mat4 model;

void main(void)
{	
	vec4 totalLocalPos = vec4(0.0);
	vec4 totalNormal = vec4(0.0);
	
	for (int i=0; i<MAX_WEIGHTS; i++)
	{
		int index = int(in_jointIndices[i]);
		//if (index == 0) continue;
		mat4 jointTransform = jointTransforms[index];

		vec4 posePosition = jointTransform * vec4(in_position, 1.0);
		totalLocalPos += posePosition * in_weights[i];
		
		vec4 worldNormal = jointTransform * vec4(in_normal, 0.0);
		totalNormal += worldNormal * in_weights[i];
	}

	gl_Position = proj * view * model * totalLocalPos;

	//gl_Position = proj * view * model * vec4(in_position, 1.0);
	pass_normals = totalNormal.xyz;
	pass_textureCoords = in_textureCoords;
	pass_weights = in_weights;
}