﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.DocumentationRules
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Analyzers.DocumentationRules;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using TestHelper;
    using Xunit;

    /// <summary>
    /// Unit tests for file header that do not follow the XML syntax.
    /// </summary>
    public class NoXmlFileHeaderUnitTests : CodeFixVerifier
    {
        private const string TestSettings = @"
{
  ""settings"": {
    ""documentationRules"": {
      ""companyName"": ""FooCorp"",
      ""copyrightText"": ""Copyright (c) {companyName}. All rights reserved.\nLicensed under the {licenseName} license. See {licenseFile} file in the project root for full license information."",
      ""variables"": {
        ""licenseName"": ""???"",
        ""licenseFile"": ""LICENSE""
      },
      ""xmlHeader"": false
    }
  }
}
";

        private string currentTestSettings = TestSettings;

        /// <summary>
        /// Verifies that the analyzer will report <see cref="FileHeaderAnalyzers.SA1633DescriptorMissing"/> for
        /// projects not using XML headers when the file is completely missing a header.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public virtual async Task TestNoFileHeaderAsync()
        {
            var testCode = @"namespace Foo
{
}
";
            var fixedCode = @"// Copyright (c) FooCorp. All rights reserved.
// Licensed under the ??? license. See LICENSE file in the project root for full license information.

namespace Foo
{
}
";

            var expectedDiagnostic = this.CSharpDiagnostic(FileHeaderAnalyzers.SA1633DescriptorMissing).WithLocation(1, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the analyzer will report <see cref="FileHeaderAnalyzers.SA1633DescriptorMissing"/> for
        /// projects not using XML headers when the file is completely missing a header.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        [WorkItem(2415, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2415")]
        public virtual async Task TestNoFileHeaderWithUsingDirectiveAsync()
        {
            var testCode = @"using System;

namespace Foo
{
}
";
            var fixedCode = @"// Copyright (c) FooCorp. All rights reserved.
// Licensed under the ??? license. See LICENSE file in the project root for full license information.

using System;

namespace Foo
{
}
";

            var expectedDiagnostic = this.CSharpDiagnostic(FileHeaderAnalyzers.SA1633DescriptorMissing).WithLocation(1, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the analyzer will report <see cref="FileHeaderAnalyzers.SA1633DescriptorMissing"/> for
        /// projects not using XML headers when the file is completely missing a header.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        [WorkItem(2415, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2415")]
        public virtual async Task TestNoFileHeaderWithBlankLineAndUsingDirectiveAsync()
        {
            var testCode = @"
using System;

namespace Foo
{
}
";
            var fixedCode = @"// Copyright (c) FooCorp. All rights reserved.
// Licensed under the ??? license. See LICENSE file in the project root for full license information.

using System;

namespace Foo
{
}
";

            var expectedDiagnostic = this.CSharpDiagnostic(FileHeaderAnalyzers.SA1633DescriptorMissing).WithLocation(1, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the analyzer will report <see cref="FileHeaderAnalyzers.SA1633DescriptorMissing"/> for
        /// projects not using XML headers when the file is completely missing a header.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        [WorkItem(2415, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2415")]
        public virtual async Task TestNoFileHeaderWithWhitespaceLineAsync()
        {
            var testCode = "    " + @"
using System;

namespace Foo
{
}
";
            var fixedCode = @"// Copyright (c) FooCorp. All rights reserved.
// Licensed under the ??? license. See LICENSE file in the project root for full license information.

using System;

namespace Foo
{
}
";

            var expectedDiagnostic = this.CSharpDiagnostic(FileHeaderAnalyzers.SA1633DescriptorMissing).WithLocation(1, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the built-in variable <c>fileName</c> works as expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public virtual async Task TestFileNameBuiltInVariableAsync()
        {
            this.currentTestSettings = @"
{
  ""settings"": {
    ""documentationRules"": {
      ""companyName"": ""FooCorp"",
      ""copyrightText"": ""{fileName} Copyright (c) {companyName}. All rights reserved.\nLicensed under the {licenseName} license. See {licenseFile} file in the project root for full license information."",
      ""variables"": {
        ""licenseName"": ""???"",
        ""licenseFile"": ""LICENSE""
      },
      ""xmlHeader"": false
    }
  }
}
";

            var testCode = @"namespace Foo
{
}
";
            var fixedCode = @"// Test0.cs Copyright (c) FooCorp. All rights reserved.
// Licensed under the ??? license. See LICENSE file in the project root for full license information.

namespace Foo
{
}
";

            var expectedDiagnostic = this.CSharpDiagnostic(FileHeaderAnalyzers.SA1633DescriptorMissing).WithLocation(1, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that a used-defined replacement variable <c>fileName</c> works as expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public virtual async Task TestFileNameUserVariableAsync()
        {
            this.currentTestSettings = @"
{
  ""settings"": {
    ""documentationRules"": {
      ""companyName"": ""FooCorp"",
      ""copyrightText"": ""{fileName} Copyright (c) {companyName}. All rights reserved.\nLicensed under the {licenseName} license. See {licenseFile} file in the project root for full license information."",
      ""variables"": {
        ""licenseName"": ""???"",
        ""licenseFile"": ""LICENSE"",
        ""fileName"": ""Not a file""
      },
      ""xmlHeader"": false
    }
  }
}
";

            var testCode = @"namespace Foo
{
}
";
            var fixedCode = @"// Not a file Copyright (c) FooCorp. All rights reserved.
// Licensed under the ??? license. See LICENSE file in the project root for full license information.

namespace Foo
{
}
";

            var expectedDiagnostic = this.CSharpDiagnostic(FileHeaderAnalyzers.SA1633DescriptorMissing).WithLocation(1, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostic, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that a file header with an auto-generated comment will not produce a diagnostic message.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestAutoGeneratedSourceFileAsync()
        {
            var testCode = @"// <auto-generated/>

namespace Bar
{
}
";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that a valid file header built using single line comments will not produce a diagnostic message.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestValidFileHeaderWithSingleLineCommentsAsync()
        {
            var testCode = @"// Copyright (c) FooCorp. All rights reserved.
// Licensed under the ??? license. See LICENSE file in the project root for full license information.

namespace Bar
{
}
";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that a valid file header built using multi-line comments will not produce a diagnostic message.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestValidFileHeaderWithMultiLineComments1Async()
        {
            var testCode = @"/* Copyright (c) FooCorp. All rights reserved.
 * Licensed under the ??? license. See LICENSE file in the project root for full license information.
 */

namespace Bar
{
}
";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that a valid file header built using multi-line comments will not produce a diagnostic message.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestValidFileHeaderWithMultiLineComments2Async()
        {
            var testCode = @"/* Copyright (c) FooCorp. All rights reserved.
   Licensed under the ??? license. See LICENSE file in the project root for full license information. */

namespace Bar
{
}
";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that a file header without text / only whitespace will produce the expected diagnostic message.
        /// </summary>
        /// <param name="comment">The comment text.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("//")]
        [InlineData("//    ")]
        public async Task TestInvalidFileHeaderWithoutTextAsync(string comment)
        {
            var testCode = $@"{comment}

namespace Bar
{{
}}
";
            var fixedCode = @"// Copyright (c) FooCorp. All rights reserved.
// Licensed under the ??? license. See LICENSE file in the project root for full license information.

namespace Bar
{
}
";

            var expected = this.CSharpDiagnostic(FileHeaderAnalyzers.SA1635Descriptor).WithLocation(1, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that an invalid file header built using single line comments will produce the expected diagnostic message.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestInvalidFileHeaderWithWrongTextAsync()
        {
            var testCode = @"// Copyright (c) BarCorp. All rights reserved.
// Licensed under the ??? license. See LICENSE file in the project root for full license information.

namespace Bar
{
}
";
            var fixedCode = @"// Copyright (c) FooCorp. All rights reserved.
// Licensed under the ??? license. See LICENSE file in the project root for full license information.

namespace Bar
{
}
";
            var expected = this.CSharpDiagnostic(FileHeaderAnalyzers.SA1636Descriptor).WithLocation(1, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override string GetSettings()
        {
            return this.currentTestSettings;
        }

        /// <inheritdoc/>
        protected sealed override IEnumerable<DiagnosticAnalyzer> GetCSharpDiagnosticAnalyzers()
        {
            yield return new FileHeaderAnalyzers();
        }

        /// <inheritdoc/>
        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new FileHeaderCodeFixProvider();
        }
    }
}
