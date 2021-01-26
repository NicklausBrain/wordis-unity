using System;

namespace DictPreprocessor
{
    [Flags]
    public enum WordCategory
    {
        None,

        /// <summary>
        /// A noun is a word that identifies something; it is an “it”. Nouns can be concrete (book) or abstract (love) or collective (a pride of lions).
        /// </summary>
        Noun,

        /// <summary>
        /// A pronoun is a word used in place of a noun. It is used when the noun is already known and it is used usually to avoid repeating the noun. Personal pronouns are the most recognised, such as he or she or it. Pronouns can also be possessive (mine) and reflexive (myself).
        /// </summary>
        Pronoun,

        /// <summary>
        /// A verb describes an action. Verbs change form depending on tense, number and person. There are many irregular verbs which cause difficulties for learners. A transitive verb needs an object, while an intransitive verb doesn’t
        /// </summary>
        Verb,

        /// <summary>
        /// An adjective is a word that gives extra information to describe a noun. Adjectives can be used before the noun (attributively) or after the noun (predicatively).
        /// </summary>
        Adjective,

        /// <summary>
        /// An adverb is a word which is used to describe an adjective or a verb. It usually ends in –ly (quickly). There are many different kinds of adverbs – of time, manner and place.
        /// </summary>
        Adverb,

        /// <summary>
        /// A preposition is used with a noun or pronoun to show its relationship to something else. Prepositions can describe position (opposite), manner (by) and time (on Sunday).
        /// </summary>
        Preposition,

        /// <summary>
        /// A conjunction is a word which connects phrases, clauses or sentences. A co-ordinating conjunction joins items of equal value (or, and), while a subordinating conjunction joins a subordinate to a main clause (until, before).
        /// </summary>
        Conjunction,

        /* A determiner is a word which introduces a noun. Determiners are also known as articles .The determiner the is the definite article, while the determiner a is the indefinite article. */
        Determiner,
    }
}