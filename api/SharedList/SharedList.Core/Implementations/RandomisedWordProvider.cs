using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharedList.Core.Abstractions;
using SharedList.Core.Extensions;

namespace SharedList.Core.Implementations
{
    public class RandomisedWordProvider : IRandomisedWordProvider
    {
        private readonly IList<string> _adjectives;
        private readonly IList<string> _desserts;
        private readonly IList<string> _animals;
        private readonly Random _random;

        public RandomisedWordProvider(Random random)
        {
            _random = random;

            var basePath = Path.Combine(AppContext.BaseDirectory, "Resources");
            _adjectives = File.ReadAllLines(Path.Combine(basePath, "adjectives.txt"));
            _desserts = File.ReadAllLines(Path.Combine(basePath, "desserts.txt"));
            _animals = File.ReadAllLines(Path.Combine(basePath, "animals.txt"));
        }

        public string CreateWordsString()
        {
            // pick two random adjectives
            // then pick either an animal or a dessert
            var adj1 = _adjectives[_random.Next(0, _adjectives.Count - 1)].Capitalise();
            var adj2 = _adjectives[_random.Next(0, _adjectives.Count - 1)].Capitalise();
            var noun = _random.Next(0, 2) == 1
                ? _animals[_random.Next(0, _animals.Count - 1)]
                : _desserts[_random.Next(0, _desserts.Count - 1)];

            return adj1 + adj2 + noun; 
        }
    }
}
