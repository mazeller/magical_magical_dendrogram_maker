#ifndef NEEDLEMAN_WUNSCH_H
#define NEEDLEMAN_WUNSCH_H

#include <stddef.h> 

// Export macro for cross-platform support
#ifdef _WIN32
#define EXPORT __declspec(dllexport)
#else
#define EXPORT
#endif

/**
 * Performs a global sequence alignment using the Needleman-Wunsch algorithm.
 *
 * @param string1       First input sequence
 * @param string2       Second input sequence
 * @param matchScore    Score for character match
 * @param mismatchScore Score for character mismatch
 * @param gapPenalty    Penalty for inserting a gap
 *
 * @return A heap-allocated C string containing two aligned sequences separated by a newline.
 *         The caller is responsible for freeing this memory with free().
 */
    EXPORT char* needlemanWunsch(const char* string1,
        const char* string2,
        int matchScore,
        int mismatchScore,
        int gapPenalty);

#endif  // NEEDLEMAN_WUNSCH_H
