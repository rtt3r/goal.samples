// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace Goal.Samples.Identity.Pages.Ciba
{
    public class InputModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
    }
}