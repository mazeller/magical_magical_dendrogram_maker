#include "needleman_wunsch.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

    __declspec(dllexport)
        char* needlemanWunsch(const char* string1, const char* string2, int matchScore, int mismatchScore, int gapPenalty)
    {
        int m = strlen(string1);
        int n = strlen(string2);

        int* score = (int*)malloc((m + 1) * (n + 1) * sizeof(int));
        char* direction = (char*)malloc((m + 1) * (n + 1) * sizeof(char));
        if (!score || !direction) return NULL;

#define IDX(i, j) ((i) * (n + 1) + (j))

        // Initialize matrices
        for (int i = 0; i <= m; i++) {
            score[IDX(i, 0)] = i * gapPenalty;
            direction[IDX(i, 0)] = 't';
        }
        for (int j = 0; j <= n; j++) {
            score[IDX(0, j)] = j * gapPenalty;
            direction[IDX(0, j)] = 'l';
        }
        direction[IDX(0, 0)] = '0';

        // Fill matrices
        for (int i = 1; i <= m; i++) {
            for (int j = 1; j <= n; j++) {
                int diag = score[IDX(i - 1, j - 1)] +
                    (toupper(string1[i - 1]) == toupper(string2[j - 1]) ? matchScore : mismatchScore);
                int insert = score[IDX(i - 1, j)] + gapPenalty;
                int delete = score[IDX(i, j - 1)] + gapPenalty;

                int max = diag;
                direction[IDX(i, j)] = 'd';

                if (insert > max) {
                    max = insert;
                    direction[IDX(i, j)] = 't';
                }
                if (delete > max || delete == max) {
                    max = delete;
                    direction[IDX(i, j)] = 'l';
                }

                score[IDX(i, j)] = max;
            }
        }

        // Traceback
        char* alnString1 = (char*)malloc(m + n + 1);
        char* alnString2 = (char*)malloc(m + n + 1);
        int i = m, j = n, idx = 0;

        while (i + j > 0) {
            char dir = direction[IDX(i, j)];
            if (dir == 'd') {
                alnString1[idx] = string1[i - 1];
                alnString2[idx] = string2[j - 1];
                i--; j--;
            }
            else if (dir == 'l') {
                alnString1[idx] = '-';
                alnString2[idx] = string2[j - 1];
                j--;
            }
            else { // 't'
                alnString1[idx] = string1[i - 1];
                alnString2[idx] = '-';
                i--;
            }
            idx++;
        }

        alnString1[idx] = '\0';
        alnString2[idx] = '\0';

        // Reverse both alignments
        for (int k = 0; k < idx / 2; k++) {
            char tmp = alnString1[k];
            alnString1[k] = alnString1[idx - 1 - k];
            alnString1[idx - 1 - k] = tmp;

            tmp = alnString2[k];
            alnString2[k] = alnString2[idx - 1 - k];
            alnString2[idx - 1 - k] = tmp;
        }

        // Combine into single string (two lines)
        int len = strlen(alnString1) + strlen(alnString2) + 2;
        char* result = (char*)malloc(len);
        snprintf(result, len, "%s\n%s", alnString1, alnString2);

        free(score);
        free(direction);
        free(alnString1);
        free(alnString2);

        return result;
    }