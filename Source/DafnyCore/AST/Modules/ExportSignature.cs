using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Microsoft.Dafny;

public class ExportSignature : TokenNode, IHasReferences {
  public readonly IToken ClassIdTok;
  public readonly bool Opaque;
  public readonly string ClassId;
  public readonly string Id;

  [FilledInDuringResolution] public Declaration Decl;

  [ContractInvariantMethod]
  void ObjectInvariant() {
    Contract.Invariant(Tok != null);
    Contract.Invariant(Id != null);
    Contract.Invariant((ClassId != null) == (ClassIdTok != null));
  }

  public ExportSignature(IToken prefixTok, string prefix, IToken idTok, string id, bool opaque) {
    Contract.Requires(prefixTok != null);
    Contract.Requires(prefix != null);
    Contract.Requires(idTok != null);
    Contract.Requires(id != null);
    tok = idTok;
    ClassIdTok = prefixTok;
    ClassId = prefix;
    Id = id;
    Opaque = opaque;
    OwnedTokensCache = new List<IToken>() { Tok, prefixTok };
  }

  public ExportSignature(IToken idTok, string id, bool opaque) {
    Contract.Requires(idTok != null);
    Contract.Requires(id != null);
    tok = idTok;
    Id = id;
    Opaque = opaque;
    OwnedTokensCache = new List<IToken>() { Tok };
  }

  public ExportSignature(Cloner cloner, ExportSignature original) {
    tok = cloner.Tok(original.Tok);
    Id = original.Id;
    Opaque = original.Opaque;
    ClassId = original.ClassId;
    ClassIdTok = cloner.Tok(original.ClassIdTok);
    OwnedTokensCache = new List<IToken>() { Tok };
  }

  public override string ToString() {
    if (ClassId != null) {
      return ClassId + "." + Id;
    }
    return Id;
  }

  public IToken NavigationToken => Tok;
  public override IEnumerable<INode> Children => Enumerable.Empty<Node>();
  public override IEnumerable<INode> PreResolveChildren => Enumerable.Empty<Node>();
  public IEnumerable<IHasNavigationToken> GetReferences() {
    return new[] { Decl };
  }
}