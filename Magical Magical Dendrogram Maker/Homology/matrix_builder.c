#include "needleman_wunsch.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

static double calculate_similarity(const char* alignedSeq1, const char* alignedSeq2);
static int hammingDistance(const char* seqA, const char* seqB);

__declspec(dllexport)
double* buildSimilarityMatrix(const char** sequences, int count, int match, int mismatch, int gap)
{
    double* matrix = malloc(count * count * sizeof(double));
    if (!matrix) return NULL;

    for (int i = 0; i < count; i++) {
        for (int j = i; j < count; j++) {
            char* result = needlemanWunsch(sequences[i], sequences[j], match, mismatch, gap);
            if (!result) {
				matrix[i * count + j] = matrix[j * count + i] = 0.0;
				continue;
            }

            char* newline = strchr(result, '\n');
            if (!newline) {
                matrix[i * count + j] = matrix[j * count + i] = 0.0;
                free(result);
                continue;
            }

            *newline = '\0';
            const char* alignedSeq1 = result;
            const char* alignedSeq2 = newline + 1;

            double score = calculate_similarity(alignedSeq1, alignedSeq2);
            matrix[i * count + j] = matrix[j * count + i] = score;

            free(result);
        }
    }
    return matrix;
}

double calculate_similarity(const char* alignedSeq1, const char* alignedSeq2)
{
    int dist = hammingDistance(alignedSeq1, alignedSeq2);
    if (dist < 0) return 0.0;

    int len = strlen(alignedSeq1);
    if (len == 0) return 0.0;

    double matches = len - dist;
    double percent = (matches / len) * 100.0;
    return percent;
}

int hammingDistance(const char* seqA, const char* seqB)
{
    int distance = 0;
    size_t lenA = strlen(seqA);
    size_t lenB = strlen(seqB);

    if (lenA != lenB)
        return -1;

    for (size_t i = 0; i < lenA; i++) {
        if (toupper(seqA[i]) != toupper(seqB[i]))
            distance++;
    }

    return distance;
}